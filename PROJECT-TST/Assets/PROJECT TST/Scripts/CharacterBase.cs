using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
                ResetOptions(); // 추후 바뀔 수 있을듯?
                weapon.gameObject.SetActive(isArmed);
            }
        }

        private bool isArmed = false;

        public Animator animator;
        public UnityEngine.CharacterController unityCharacterController;
        public WeaponBase weapon;

        public Vector3 offsetPosition;
        public Vector3 offsetRotation;

        public float walkSpeed = 1f;
        public float moveSpeed = 2f;
        public float runSpeed = 2.1f;
        public float sprintSpeed = 5f;
        public float rotateSpeed = 5f;

        private float horizontal;
        private float vertical;
        private float speedBlend;
        private float armedBlend;
        private float reloadBlend;

        private float targetSpeed;
        private float targetHorizontal;
        private float targetVertical;

        #region Tory
        // FSM 으로 추후 변경이 필요해보임
        public bool IsSprint
        {
            get => isSprint;
            set
            {
                isSprint = value;
            }
        }

        public bool IsWalkMode
        {
            get => isWalkMode;
            set
            {
                isWalkMode = value;
            }
        }

        // isReload 장전중이냐?
        public bool IsReload
        {
            get => isReload;
            set
            {
                isReload = value;
            }
        }

        private bool isSprint = false;
        private bool isWalkMode = false;
        private bool isReload = false;
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
            reloadBlend = Mathf.Lerp(reloadBlend, IsReload ? 1f : 0f, Time.deltaTime * 3f);

            animator.SetFloat("Armed", armedBlend);
            animator.SetFloat("Speed", speedBlend);
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
            animator.SetFloat("Reload", reloadBlend);
            animator.SetLayerWeight(1, reloadBlend);
        }

        public void Move(Vector2 input)
        {
            if (input.magnitude > 0f)
            {
                targetSpeed = IsWalkMode ? walkSpeed : runSpeed;
                targetHorizontal = input.x;
                targetVertical = input.y;

                // 수정 부분
                Vector3 movement =  (transform.forward * input.y + transform.right * input.x) 
                    * (IsSprint && !IsWalkMode ? sprintSpeed : moveSpeed) * Time.deltaTime;
                unityCharacterController.Move(movement);
            }
            else
            {
                targetSpeed = 0f;
                targetHorizontal = 0f;
                targetVertical = 0f;
            }
        }

        private void ResetOptions()
        {
            // 기본적으로 달리기 모드 설정
            isWalkMode = false;
        }

        public void Rotate(float rotation)
        {
            transform.Rotate(Vector3.up * rotation * rotateSpeed * Time.deltaTime);
        }

        public void Shoot()
        {
            // 만약 총알이 0발이라면? Reload 
            // 리로드 중 아닐때만
            if (!IsReload)
                weapon.Fire();
            
            if (weapon.CurrentAmmo <= 0)
                Reload();
        }

        public void Reload()
        {
            weapon.Reload();
            IsReload = true;
        }

        private void ReloadComplete()
        {
            IsReload = false;
        }
    }
}