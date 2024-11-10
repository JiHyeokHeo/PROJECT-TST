using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TST
{
    public class CharacterBase : MonoBehaviour
    {
        public Vector3 AimingPosition
        {
            get => aimingPoint.position;
            set => aimingPoint.position = value;
        }

        public bool IsArmed 
        {
            get => isArmed;
            set
            {
                isArmed = value;
                SetEquipWeapon(isArmed); // 추후 바뀔 수 있을듯?
            }
        }

        private bool isArmed = false;
        private bool isArmedCompleted = false;

        public Animator animator;
        public UnityEngine.CharacterController unityCharacterController;
        public Transform cameraPivot;

        public CinemachineGunRecoil cameraGunRecoilComponent;
        public WeaponBase weapon;
        public Transform weaponSocket;
        public Transform weaponHolder;
        public Transform aimingPoint;

        public RigBuilder rigBuilder;
        public Rig aimingRig;
        public Rig lefthandRig;

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
        private float Arm_HeadBlend;

        private float targetSpeed;
        private float targetHorizontal;
        private float targetVertical;

        private bool isReloading = false;

        private float aimingRigWeightBlend;
        private float lefthandRigWeightBlend;

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

        [field : SerializeField] private bool isSprint = true;
        private bool isAutoRunMode = false;
        private bool isWalk = false;
        private bool isRollFinished = true;
        #endregion

        private void Awake()
        {
            animator = GetComponent<Animator>();
            unityCharacterController = GetComponent<UnityEngine.CharacterController>();

            // 구르기
        }

        private void Start()
        {
            aimingRig.weight = 0f;
            lefthandRig.weight = 0f;
            rigBuilder.Build();
        }

        public float Whole_Body_Weight_Blend = 0.0f;
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

        private void LateUpdate()
        {
            aimingRigWeightBlend = Mathf.Lerp(aimingRigWeightBlend, isArmedCompleted ? 1f : 0f, Time.deltaTime * 10f);
            aimingRig.weight = aimingRigWeightBlend;

            lefthandRigWeightBlend = Mathf.Lerp(lefthandRigWeightBlend, isArmedCompleted && !isReloading ? 1f : 0f, Time.deltaTime * 10f);
            lefthandRig.weight = lefthandRigWeightBlend;
        }

        private float targetRotation = 0f;

        public void Move(Vector2 input, float yAxisAngle)
        {
            if (!isRollFinished)
                return;

            if (input.magnitude > 0f)
            {
                if (!IsArmed)
                {
                    Vector3 inputDirection = new Vector3(input.x, 0f, input.y);
                    targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + yAxisAngle;
                    transform.rotation = Quaternion.Euler(0f, targetRotation, 0f);
                }

                Vector3 movement = Vector3.zero;

                if (IsArmed)
                {
                    targetHorizontal = input.x;
                    targetVertical = input.y;
                    movement = (transform.forward * input.y + transform.right * input.x) * moveSpeed * Time.deltaTime;
                }
                else
                {
                    targetVertical = 1f;
                    movement = transform.forward * moveSpeed * Time.deltaTime;
                }

                unityCharacterController.Move(movement);
            }
            else
            {
                targetHorizontal = 0f;
                targetVertical = 0f;
            }

            //if (input.magnitude > 0f || IsAutoRunMode)
            //{
            //    // 자동달리기 켜져있으면 일단 스프린트 모드 On
            //    targetSpeed = IsSprint ? 1.0f : 0.0f;
            //    targetHorizontal = input.x;
            //    targetVertical = IsAutoRunMode ? 1.0f : input.y;

            //    Vector3 movement =  (transform.forward * targetVertical + transform.right * targetHorizontal) 
            //        * (IsSprint ? sprintSpeed : moveSpeed) * Time.deltaTime;
            //    unityCharacterController.Move(movement);
            //}
            //else
            //{
            //    IsSprint = true;
            //    targetSpeed = IsAutoRunMode ? targetSpeed : 0f;
            //    targetHorizontal = IsAutoRunMode ? targetHorizontal : 0f;
            //    targetVertical = IsAutoRunMode ? targetVertical : 0f;
            //}

            if (!isAutoRunMode)
                animator.SetFloat("Magnitude", input.magnitude);
            else 
                animator.SetFloat("Magnitude", 1.0f);

        }

        //public float rollSpeed = 4.0f;
        //private IEnumerator rollCoroutine;
        //IEnumerator StartRollCoroutine()
        //{
        //    while (true)
        //    {
        //        if (isRollFinished)
        //            StopCoroutine(rollCoroutine);

        //        Vector3 movement = (transform.forward * 1.0f + transform.right * 0.0f)
        //            * rollSpeed * Time.deltaTime;
        //        unityCharacterController.Move(movement);
        //        yield return null;
        //    }
        //}

        public void Roll()
        {
            if (isRollFinished)
            {
                animator.SetTrigger("Roll Trigger");
                isRollFinished = false;
            }
        }

        public void Rotate(Vector3 targetPoint)
        {
            if (!isRollFinished)
                return;

            if (IsArmed)
            {
                Vector3 target = targetPoint;
                target.y = transform.position.y;
                Vector3 pos = transform.position;
                Vector3 direction = (target - pos).normalized;

                transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 10f);
            }
        }


        public void Shoot()
        {
            if (IsArmed && isArmedCompleted && isRollFinished)
            {
                bool isFireSuccess = weapon.Fire();
                if (!isFireSuccess && weapon.CurrentAmmo <= 0)
                {
                    Reload();
                    cameraGunRecoilComponent.PauseRecoil();
                    return;
                }

                if (isFireSuccess)
                    cameraGunRecoilComponent.StartRecoil();
            }
        }

        public void ShootFinished()
        {
            cameraGunRecoilComponent.PauseRecoil();
        }

        public void Reload()
        {
            // # 재장전 애니메이션 Trigger 호출
            // TODO : 이미 재장전을 하고 있었다면? 재장전을 하지 않도록 예외처리하자.
            if (!isReloading && weapon.CurrentAmmo != weapon.clipSize)
            {
                isReloading = true;
                animator.SetTrigger("Reload Trigger");
            }
        }

        public void SetReloadComplete()
        {
            // # 재장전 애니메이션 완료시 호출 되는 구역
            // TODO : WeaponBase에 총알을 다시 가득채운다.
            weapon.Reload();
            isReloading = false;
        }

        private void SetEquipWeapon(bool isArmed)
        {
            if (isArmed)
            {
                animator.SetTrigger("Equip Trigger");
            }
            else
            {
                animator.SetTrigger("Holster Trigger");
            }
        }

        public void SetEquipmentVisual(int activated)
        {
            if (activated == 1)
            {
                weapon.transform.SetParent(weaponHolder);
                weapon.transform.localPosition = offsetPosition;
                weapon.transform.localRotation = Quaternion.Euler(offsetRotation);
            }
            else
            {
                weapon.transform.SetParent(weaponSocket);
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
            }
        }

        public void RollingFinished(int flag)
        {
            isRollFinished = flag > 0;
        }

        public void SetArmedComplete(int flag)
        {
            isArmedCompleted = flag > 0;
        }
    }
}