using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    [CreateAssetMenu(fileName = "RecoilNoiseSettings", menuName = "ScriptableObjects/RecoilNoiseSettings")]
    public class RecoilNoiseSettings : ScriptableObject
    {
        public GameObject gun;
        public string gunName = string.Empty;
        public AnimationCurve positionXCurve;
        public AnimationCurve positionYCurve;
        public float frequencyX = 1f; 
        public float frequencyY = 1f;
        public float amplitudeX = 1f; // Recoil Èçµé¸² ¼öÄ¡
        public float amplitudeY = 1f; // Recoil Èçµé¸² ¼öÄ¡
    }
}
