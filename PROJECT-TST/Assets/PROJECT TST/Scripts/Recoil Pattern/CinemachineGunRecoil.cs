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

        bool isFire;
        // Amplitude를 나는 value값으로 설정하는 것이 좋아보인다.
        // Frequency를 통해 키고 끄는 걸 정해주자.
        void Start()
        {
            noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noiseComponent.m_FrequencyGain = 0.0f;
            noiseComponent.m_AmplitudeGain = 0.0f;
            PauseRecoil();
        }

        public void PauseRecoil()
        {
            shotCnt = 0;
            isFire = false;
            timeElapsed = 0;
        }

        int shotCnt = 0;
        public void StartRecoil()
        {
            // 첫발부터 recoil 시작
            shotCnt++;
            if (shotCnt == 1)
            {
                noiseComponent.m_NoiseProfile.PositionNoise[0].X.Amplitude = 0.0f;
                noiseComponent.m_NoiseProfile.PositionNoise[0].Y.Amplitude = 0.0f;
                timeElapsed = 0.0f;
            }

            isFire = true;
            noiseComponent.m_FrequencyGain = 1.0f;
            noiseComponent.m_AmplitudeGain = 1.0f;
        }

        void Update()
        {
            if (noiseComponent == null)
                return;

            RecoilCheck();
        }

        void RecoilCheck()
        {
            if (virtualCamera != null)
            {
                timeElapsed += Time.deltaTime;

                noisePositionSet(timeElapsed);
            }
        }

        void noisePositionSet(float timeElapsed)
        {
            noiseComponent.m_NoiseProfile.PositionNoise[0].X.Frequency = recoilSetting.frequencyX;
            noiseComponent.m_NoiseProfile.PositionNoise[0].X.Amplitude = Mathf.Lerp(noiseComponent.m_NoiseProfile.PositionNoise[0].X.Amplitude, isFire ? recoilSetting.positionXCurve.Evaluate(timeElapsed) : 0.0f, Time.deltaTime * 10.0f);
            noiseComponent.m_NoiseProfile.PositionNoise[0].Y.Frequency = recoilSetting.frequencyY;
            noiseComponent.m_NoiseProfile.PositionNoise[0].Y.Amplitude = Mathf.Lerp(noiseComponent.m_NoiseProfile.PositionNoise[0].Y.Amplitude, isFire ? recoilSetting.positionYCurve.Evaluate(timeElapsed) : 0.0f, Time.deltaTime * 10.0f); 
        }

        
    }
}
