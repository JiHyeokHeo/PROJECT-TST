using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
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
        public Rigidbody[] ragdollRigidbodies;

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
        private bool isRolling = false;
        #endregion

        private void Awake()
        {
            animator = GetComponent<Animator>();
            unityCharacterController = GetComponent<UnityEngine.CharacterController>();
            ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            SetRagdollActive(false);
            // 구르기
        }

        public void SetRagdollActive(bool isActive)
        {
            foreach (var rb in ragdollRigidbodies)
            {
                rb.isKinematic = !isActive;
            }

            animator.enabled = !isActive;
            unityCharacterController.enabled = !isActive;
        }

        private void Start()
        {
            aimingRig.weight = 0f;
            lefthandRig.weight = 0f;
            rigBuilder.Build();

            //StartCoroutine(DelayedActiveRagdoll());
            //IEnumerator DelayedActiveRagdoll()
            //{
            //    yield return new WaitForSeconds(3f);
            //    SetRagdollActive(true);
            //}
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

            if (isRolling)
                StartRoll();
        }

        private void LateUpdate()
        {
            aimingRigWeightBlend = Mathf.Lerp(aimingRigWeightBlend, isArmedCompleted && !isRolling ? 1f : 0f, Time.deltaTime * 10f);
            aimingRig.weight = aimingRigWeightBlend;

            lefthandRigWeightBlend = Mathf.Lerp(lefthandRigWeightBlend, isArmedCompleted && !isReloading && !isRolling ? 1f : 0f, Time.deltaTime * 10f);
            lefthandRig.weight = lefthandRigWeightBlend;
        }

        private float targetRotation = 0f;

        public void Move(Vector2 input, float yAxisAngle)
        {
            if (isRolling)
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

        public float rollSpeed = 4.0f;
        private float rollTime;
        public AnimationCurve rollSpeedCurve;
        private void StartRoll()
        {
            rollTime += Time.deltaTime;
            Vector3 movement = (transform.forward * 1.0f + transform.right * 0.0f)
                    * (rollSpeed * rollSpeedCurve.Evaluate(rollTime) * Time.deltaTime);
            unityCharacterController.Move(movement);
        }

        public void Roll()
        {
            if (!isRolling)
            {
                animator.SetTrigger("Roll Trigger");
                isRolling = true;
            }
        }

        public bool Rotate(Vector3 targetPoint)
        {
            // 타겟은 일단 에이밍 걸린 포인트이다
            if (isRolling)
                return false;

            // 내적 = 각 벡터의 길이 * cos세타
            
            if (IsArmed)
            {
                Vector3 target = targetPoint;
                target.y = transform.position.y;
                Vector3 pos = transform.position;
                Vector3 direction = (target - pos).normalized;

                Vector3 viewForward = Camera.main.transform.forward;
                viewForward.y = transform.position.y;

                float dotResult = Vector3.Dot(viewForward, direction);
                // 내적값이 음수가 나오면 forward를 카메라 정면 방향으로 변경
                // targetPoint와 플레이어의 거리에 따라 예외처리가 필요할지..?
                if (dotResult < 0.9)
                {
                    transform.forward = Vector3.Lerp(transform.forward, viewForward, Time.deltaTime * 10f);
                    return false;
                }

                transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 10f);
            }

            return true;
        }

        public void Shoot()
        {
            if (isRolling)
                return;

            if (IsArmed && isArmedCompleted)
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

        //public void ZipLine()
        //{

        //}

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
            isRolling = false;
            rollTime = 0.0f;
        }

        public void SetArmedComplete(int flag)
        {
            isArmedCompleted = flag > 0;
        }
    }
}