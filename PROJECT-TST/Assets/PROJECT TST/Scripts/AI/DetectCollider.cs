using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class DetectCollider : MonoBehaviour
    {
        public CharacterBase characterBase;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDetect detectInterface))
            {
                detectInterface.Detect(characterBase.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // ≈ª√‚«œ∏È UNDECT
            if (other.TryGetComponent(out IDetect detectInterface))
            {
                detectInterface.UnDetect(characterBase.gameObject);
            }
        }
    }
}
