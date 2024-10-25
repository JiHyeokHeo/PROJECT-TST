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

        private float pitch = 0f;

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
            Debug.Log(mouseY);
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

            pitch -= mouseY;
            cameraPivot.localRotation = Quaternion.Euler(Mathf.Clamp(pitch, bottomClampLimit, topClampLimit), 0, 0);

            linkedCharacter.Move(new Vector2(inputX, inputY));
            linkedCharacter.Rotate(mouseX);
        }
    }
}

