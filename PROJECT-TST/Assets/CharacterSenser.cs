using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class CharacterSenser : MonoBehaviour
    {

        public event System.Action<CharacterBase> OnDetectedCharacter;
        public event System.Action<CharacterBase> OnLostCharacter;

        private void OnTriggerEnter(Collider other)
        {
            CharacterBase detectedCharacter = null;
            OnDetectedCharacter?.Invoke(detectedCharacter);
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterBase detectedCharacter = null;
            OnLostCharacter?.Invoke(detectedCharacter);
        }
    }
}
