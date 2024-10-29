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

        public bool IsAutoRunMode
        {
            get => isAutoRunMode;
            set
            {
                if (value == false)
                    IsWalk = false;
                isAutoRunMode = value;
            }
        }

        public bool IsWalk
        {
            get => isWalk;
            set
            {
                // 자동 달리기 모드일 때만
                if (IsAutoRunMode)
                    isWalk = value;
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
        private bool isAutoRunMode = false;
        private bool isReload = false;
        private bool isWalk = false;
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
            if (IsAutoRunMode)
            {
                targetSpeed = !IsWalk ? runSpeed : walkSpeed;
                targetHorizontal = 0f;
                targetVertical = 1.0f;

                // 오직 정면만 돌진 // 이친구는 Run(스프린트 스피드) or Walk(워크스피드) 모드
                Vector3 movement = (transform.forward * 1.0f)
                * (!IsWalk ? sprintSpeed : moveSpeed) * Time.deltaTime;
                unityCharacterController.Move(movement);
            }

            if (input.magnitude > 0f && !IsAutoRunMode)
            {
                // 자동달리기 켜져있으면 일단 스프린트 모드 On
                targetSpeed = IsSprint ? runSpeed : walkSpeed;
                targetHorizontal = input.x;
                targetVertical = input.y;

                Vector3 movement =  (transform.forward * input.y + transform.right * input.x) 
                    * (IsSprint ? sprintSpeed : moveSpeed) * Time.deltaTime;
                unityCharacterController.Move(movement);
            }
            else
            {
                IsSprint = false;
                targetSpeed = IsAutoRunMode ? targetSpeed : 0f;
                targetHorizontal = IsAutoRunMode ? targetHorizontal : 0f;
                targetVertical = IsAutoRunMode ? targetVertical : 0f;
            }
        }

        private void ResetOptions()
        {
            // 기본적으로 달리기 모드 설정
            //isWalk = false;
        }

        public void Rotate(float rotation)
        {
            transform.Rotate(Vector3.up * rotation * rotateSpeed * Time.deltaTime);
        }

        public void Shoot()
        {
            // 만약 총알이 0발이라면? Reload 
            // 리로드 중 아닐때만
            if (IsReload)
                return;
            
            weapon.Fire();
            if (weapon.CurrentAmmo <= 0)
                Reload();
        }

        public void Reload()
        {
            weapon.Reload();
            IsReload = true;
        }

        // 애니메이션 이벤트
        private void ReloadComplete()
        {
            IsReload = false;
        }
    }
}