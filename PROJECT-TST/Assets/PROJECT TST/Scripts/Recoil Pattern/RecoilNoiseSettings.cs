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
        public AnimationCurve rotationXCurve;
        public AnimationCurve rotationYCurve;
        public float frequencyX = 1f;
        public float frequencyY = 1f;
        public float amplitudeX = 1f;
        public float amplitudeY = 1f;
    }
}
