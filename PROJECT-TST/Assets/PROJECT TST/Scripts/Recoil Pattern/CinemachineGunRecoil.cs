using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class CinemachineGunRecoil : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;
        public RecoilNoiseSettings recoilNoiseSettings;
        public GameObject player;

        private float recoilTimeX;
        private float recoilTimeY;
        private CinemachineBasicMultiChannelPerlin noiseComponent;
        private CharacterBase characterBase;
        void Start()
        {
            // Cinemachine 카메라에서 노이즈 컴포넌트 가져오기
            noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            if (noiseComponent == null)
            {
                Debug.LogWarning("CinemachineBasicMultiChannelPerlin component not found on the Virtual Camera. Please add it in the Noise section.");
            }

            characterBase = player.GetComponent<CharacterBase>();
        }

        void Update()
        {
            if (noiseComponent == null || !characterBase.IsShooting)
            {
                //recoilTimeX = 0;
                //recoilTimeY = 0;
                return;
            }
            
            // X축 노이즈 계산
            float noiseX = Mathf.PerlinNoise(recoilTimeX * recoilNoiseSettings.frequencyX, 0f);
            float offsetX = recoilNoiseSettings.rotationXCurve.Evaluate(noiseX) * recoilNoiseSettings.amplitudeX;

            // Y축 노이즈 계산
            float noiseY = Mathf.PerlinNoise(0f, recoilTimeY * recoilNoiseSettings.frequencyY);
            float offsetY = recoilNoiseSettings.rotationYCurve.Evaluate(noiseY) * recoilNoiseSettings.amplitudeY;

            // Cinemachine 노이즈 컴포넌트에 값 적용
            noiseComponent.m_AmplitudeGain = offsetX;  // X축 회전의 진폭을 AmplitudeGain으로 사용
            noiseComponent.m_FrequencyGain = offsetY;  // Y축 회전의 빈도를 FrequencyGain으로 사용

            // 시간 누적
            recoilTimeX += Time.deltaTime;
            recoilTimeY += Time.deltaTime;
        }
    }
}
