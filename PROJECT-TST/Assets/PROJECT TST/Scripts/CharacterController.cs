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

            if (Input.GetKeyDown(KeyCode.F))
            {
                for (int i = 0; i < currentInteractables.Count; i++)
                {
                    currentInteractables[i].Interact();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OptionManager.Instance.IsGameStopped = !OptionManager.Instance.IsGameStopped;
            }

            //pitch = Mathf.Clamp(pitch - mouseY * Time.deltaTime * 400.0f, bottomClampLimit, topClampLimit);
            //cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);

            Vector3 aimingPoint = Vector3.zero;
            Ray screenCenterRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Debug.DrawRay(screenCenterRay.origin, screenCenterRay.direction * 100.0f, Color.red);
            if (Physics.Raycast(screenCenterRay, out RaycastHit hitInfo, 1000f, aimingLayer, QueryTriggerInteraction.Ignore))
            {
                aimingPoint = hitInfo.point;
            }
            else
            {
                aimingPoint = screenCenterRay.GetPoint(1000f);
            }

            linkedCharacter.Move(new Vector2(inputX, inputY), Camera.main.transform.eulerAngles.y);
            bool rotateSuccess = linkedCharacter.Rotate(aimingPoint);
            linkedCharacter.AimingPosition = rotateSuccess ? aimingPoint : screenCenterRay.GetPoint(1000f);
        }


        public float interactionRange = 2f;
        public List<IInteractable> currentInteractables = new List<IInteractable>();

        private void FixedUpdate()
        {
            Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, interactionRange);
            for (int i = 0; i < overlappedObjects.Length; i++)
            {
                if (overlappedObjects[i].TryGetComponent(out IInteractable interactable))
                {
                    if (false == currentInteractables.Contains(interactable))
                    {
                        currentInteractables.Add(interactable);
                    }
                }
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        public float topClampLimit = 80;
        public float bottomClampLimit = -80;

        private float threshold = 0.01f;
        private float targetYaw;
        private float targetPitch;

        private void CameraRotation()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector2 look = new Vector2(mouseX, mouseY);

            if (look.sqrMagnitude > threshold)
            {
                float yaw = look.x;
                float pitch = -look.y;

                targetYaw = ClampAngle(targetYaw + yaw, float.MinValue, float.MaxValue);
                targetPitch = ClampAngle(targetPitch + pitch, bottomClampLimit, topClampLimit);
            }

            linkedCharacter.cameraPivot.transform.rotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle += 360;
            }

            if (angle > 360)
            {
                angle -= 360;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}

