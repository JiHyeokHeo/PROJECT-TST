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

            cameraPivot.eulerAngles = new Vector3(
                Mathf.Clamp(cameraPivot.eulerAngles.x - mouseY, bottomClampLimit, topClampLimit),
                cameraPivot.eulerAngles.y,
                cameraPivot.eulerAngles.z);

            linkedCharacter.Move(new Vector2(inputX, inputY));
            linkedCharacter.Rotate(mouseX);
        }
    }
}

