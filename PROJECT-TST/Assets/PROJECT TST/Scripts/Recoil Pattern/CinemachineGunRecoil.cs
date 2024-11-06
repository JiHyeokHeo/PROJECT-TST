using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace TST
{
     //https://docs.unity3d.com/Packages/com.unity.cinemachine@2.2/manual/CinemachineImpulseNoiseProfiles.html
     //https://discussions.unity.com/t/running-noise-profile/881047/2

    public class CinemachineGunRecoil : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;
        public RecoilNoiseSettings recoilSetting;
        public float curveDuration = 1f;

        private float timeElapsed = 0f;

        CinemachineBasicMultiChannelPerlin noiseComponent;

        // Amplitude를 나는 value값으로 설정하는 것이 좋아보인다.
        // Frequency를 통해 키고 끄는 걸 정해주자.
        void Start()
        {
            // Cinemachine 카메라에서 노이즈 컴포넌트 가져오기
            noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            NoiseSettings noiseSettings = noiseComponent.m_NoiseProfile;


            if (noiseSettings != null)
            {
                // Position X (Index 0)에 접근하여 컴포넌트를 설정
                var positionXParams = noiseSettings.PositionNoise[0];

                //// 기존 컴포넌트 값 변경
                //positionXParams.X.Frequency = 1f;
                //positionXParams.Y.Amplitude = 0.1f;

                //// 새 컴포넌트 추가
                //if (positionXParams.Components.Length < 2) // 기존 컴포넌트가 2개 미만인 경우
                //{
                //    // 배열을 확장하여 새 컴포넌트를 추가
                //    var newComponents = new NoiseSettings.NoiseParams[positionXParams.Components.Length + 1];
                //    positionXParams.Components.CopyTo(newComponents, 0);
                //    newComponents[1] = new NoiseSettings.NoiseParams
                //    {
                //        Frequency = 0f,
                //        Amplitude = 0f
                //    };
                //    positionXParams.Components = newComponents;
                //}

                //// 설정한 값을 다시 Position X에 할당
                //noiseSettings.PositionNoise[0] = positionXParams;
            }
            else
            {
                Debug.LogWarning("NoiseSettings가 할당되지 않았습니다.");
            }

        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Y))
            {
                timeElapsed = 0;
                noiseComponent.m_FrequencyGain = 0.0f;
                noiseComponent.m_AmplitudeGain = 0.0f;
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                noiseComponent.m_FrequencyGain = 1.0f;
                noiseComponent.m_AmplitudeGain = 1.0f;
            }


            if (virtualCamera != null)
            {
                var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                if (noise != null)
                {
                    // 시간을 curveDuration 내에서 반복하도록 설정
                    timeElapsed += Time.deltaTime;
                    float curveTimeY = Mathf.Repeat(timeElapsed, curveDuration);
                    //AnimationCurve의 값을 PositionNoise에 설정
                    noise.m_NoiseProfile.PositionNoise[0].X.Frequency = recoilSetting.frequencyX;
                    noise.m_NoiseProfile.PositionNoise[0].X.Amplitude = recoilSetting.positionXCurve.Evaluate(timeElapsed);
                    noise.m_NoiseProfile.PositionNoise[0].Y.Frequency = recoilSetting.frequencyY;
                    noise.m_NoiseProfile.PositionNoise[0].Y.Amplitude = recoilSetting.positionYCurve.Evaluate(timeElapsed);
                }
            }
        }
    }
}
