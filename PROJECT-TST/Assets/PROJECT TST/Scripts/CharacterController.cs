using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class CharacterController : MonoBehaviour
    {
        public CharacterBase linkedCharacter;
        public Transform cameraPivot;


        public float topClampLimit = 80;
        public float bottomClampLimit = -80;

        #region Tory


        private float pitch = 0f;

        #endregion

        private void Awake()
        {
            linkedCharacter = GetComponent<CharacterBase>();
        }

        private void Update()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputY = Input.GetAxis("Vertical");

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                CameraSystem.Instance.IsCameraSideOnRight = !CameraSystem.Instance.IsCameraSideOnRight;
            }

            if (Input.GetMouseButtonDown(1))
            {
                CameraSystem.Instance.IsCameraZoom = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                CameraSystem.Instance.IsCameraZoom = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                linkedCharacter.IsArmed = !linkedCharacter.IsArmed;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                linkedCharacter.IsSprint = !linkedCharacter.IsSprint;
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                linkedCharacter.IsAutoRunMode = !linkedCharacter.IsAutoRunMode;
            }

            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                linkedCharacter.IsWalk = !linkedCharacter.IsWalk;
            }

            if (Input.GetMouseButton(0))
            {
                linkedCharacter.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                linkedCharacter.Reload();
            }

            pitch -= mouseY;
            cameraPivot.localRotation = Quaternion.Euler(Mathf.Clamp(cameraPivot.localRotation.x + pitch, bottomClampLimit, topClampLimit), 0, 0);

            linkedCharacter.Move(new Vector2(inputX, inputY));
            linkedCharacter.Rotate(mouseX);
        }
    }
}

