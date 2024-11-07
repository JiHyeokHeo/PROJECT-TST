using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class CharacterController : MonoBehaviour
    {
        public CharacterBase linkedCharacter;
        public Transform cameraPivot;

        public LayerMask aimingLayer;

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

            if (Input.GetMouseButtonUp(0))
            {
                linkedCharacter.ShootFinished();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                linkedCharacter.Reload();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                linkedCharacter.Roll();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OptionManager.Instance.IsGameStopped = !OptionManager.Instance.IsGameStopped;
            }

            pitch = Mathf.Clamp(pitch - mouseY * Time.deltaTime * 400.0f, bottomClampLimit, topClampLimit);
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);

            linkedCharacter.Move(new Vector2(inputX, inputY));
            linkedCharacter.Rotate(mouseX);

            // ºæ≈Õ 0.5f 0.5f
            Ray screenCenterRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Debug.DrawRay(screenCenterRay.origin, screenCenterRay.direction * 100.0f, Color.red);
            if (Physics.Raycast(screenCenterRay, out RaycastHit hitInfo, 1000f, aimingLayer, QueryTriggerInteraction.Ignore))
            {
                linkedCharacter.AimingPosition = hitInfo.point;
            }
            else
            {
                linkedCharacter.AimingPosition = screenCenterRay.GetPoint(1000f);
            }
        }
    }
}

