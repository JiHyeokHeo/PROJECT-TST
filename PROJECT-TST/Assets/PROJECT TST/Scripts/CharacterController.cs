using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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

            if (Input.GetMouseButton(0))
            {
                linkedCharacter.Shoot();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                linkedCharacter.IsWalk = !linkedCharacter.IsWalk;
            }

            cameraPivot.eulerAngles = new Vector3(
                Mathf.Clamp(cameraPivot.eulerAngles.x - mouseY, bottomClampLimit, topClampLimit),
                cameraPivot.eulerAngles.y,
                cameraPivot.eulerAngles.z);

            linkedCharacter.Move(new Vector2(inputX, inputY));
            linkedCharacter.Rotate(mouseX);


            Ray screenCenterRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
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

