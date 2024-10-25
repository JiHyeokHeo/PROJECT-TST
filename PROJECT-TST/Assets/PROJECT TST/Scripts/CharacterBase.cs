using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class CharacterBase : MonoBehaviour
    {
        public bool IsArmed 
        {
            get => isArmed;
            set
            {
                isArmed = value;
                weapon.gameObject.SetActive(isArmed);
            }
        }

        private bool isArmed = false;

        public Animator animator;
        public UnityEngine.CharacterController unityCharacterController;
        public WeaponBase weapon;

        public Vector3 offsetPosition;
        public Vector3 offsetRotation;

        public float moveSpeed = 2f;
        public float sprintSpeed = 5f;
        public float rotateSpeed = 5f;

        private float horizontal;
        private float vertical;
        private float speedBlend;
        private float armedBlend;

        private float targetSpeed;
        private float targetHorizontal;
        private float targetVertical;

        #region Tory
        public bool IsSprint
        {
            get => isSprint;
            set
            {
                isSprint = value;
            }
        }

        public bool IsAutoMove
        {
            get => isAutoMove;
            set
            {
                isAutoMove = value;
            }
        }

        private bool isSprint = false;
        private bool isAutoMove = false;
        #endregion

        private void Awake()
        {
            animator = GetComponent<Animator>();
            unityCharacterController = GetComponent<UnityEngine.CharacterController>();
        }

        private void Start()
        {
            Transform rightHandTransform = animator.GetBoneTransform(HumanBodyBones.RightHand);
            weapon.transform.SetParent(rightHandTransform);
            weapon.transform.SetPositionAndRotation(rightHandTransform.position + offsetPosition, rightHandTransform.rotation * Quaternion.Euler(offsetRotation));
            weapon.gameObject.SetActive(false);
        }

        private void Update()
        {
            armedBlend = Mathf.Lerp(armedBlend, IsArmed ? 1f : 0f, Time.deltaTime * 10f);
            speedBlend = Mathf.Lerp(speedBlend, targetSpeed, Time.deltaTime * 10f);
            horizontal = Mathf.Lerp(horizontal, targetHorizontal, Time.deltaTime * 10f);
            vertical = Mathf.Lerp(vertical, targetVertical, Time.deltaTime * 10f);

            animator.SetFloat("Armed", armedBlend);
            animator.SetFloat("Speed", speedBlend);
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        }

        public void Move(Vector2 input)
        {
            // 입력 or 자동 움직임 On
            if (input.magnitude > 0f || IsAutoMove)
            {
                targetSpeed = IsSprint ? moveSpeed + 0.1f : moveSpeed;
                targetHorizontal = IsAutoMove ? 1.0f : input.x;
                targetVertical = IsAutoMove ? 1.0f : input.y;

                // 수정 부분
                Vector3 movement =  (
                    ((transform.forward * (IsAutoMove ?  1.0f : input.y)) + transform.right * (IsAutoMove ? 1.0f : input.x)) 
                    * (IsSprint ? sprintSpeed : moveSpeed) * Time.deltaTime );
                unityCharacterController.Move(movement);
            }
            else
            {
                targetSpeed = 0f;
                targetHorizontal = 0f;
                targetVertical = 0f;
            }
        }

        public void Rotate(float rotation)
        {
            transform.Rotate(Vector3.up * rotation * rotateSpeed * Time.deltaTime);
        }

        public void Shoot()
        {
            weapon.Fire();
        }
    }
}