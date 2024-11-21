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
    public enum ECharacterSocket
    {
        Gun,
        Pistol,
        Grenade,
        Knife,
        None,
    }

    public class CharacterBase : MonoBehaviour
    {
        public Vector3 AimingPosition
        {
            get => aimingPoint.position;
            set => aimingPoint.position = value;
        }

        private ECharacterSocket eLastEquippedSocket;
        public bool IsArmed 
        {
            get => isArmed;
            set
            {
                // 만약 내가 장착중인 상태에서 다른 장비로 변경을 한다면?
                isArmed = value;

                if (eLastEquippedSocket != eCurrentCharacterSocket && eLastEquippedSocket == ECharacterSocket.None)
                    SetEquipWeapon(isArmed);
                else
                {
                    // 마지막에 낀 친구를 일단 홀스터에 넣고
                    SetAnimatorArmedType(eLastEquippedSocket);
                    SetEquipWeapon(false);

                    // 최근에 장착한 친구를 장착
                    SetAnimatorArmedType(eCurrentCharacterSocket);
                    SetEquipWeapon(true);
                    isArmed = true;
                }

                // 장착식 다 끝났고 IsArmed = false면 소켓 해제
                if (IsArmed == false)
                    eCurrentCharacterSocket = ECharacterSocket.None;
            }
        }

        public ECharacterSocket CharacterSocket
        {
            get => eCurrentCharacterSocket;
            set
            {
                // 과거에 꼈던 장비 기억
                if (eCurrentCharacterSocket != value)
                    eLastEquippedSocket = eCurrentCharacterSocket;

                eCurrentCharacterSocket = value;
                SetAnimatorArmedType(eCurrentCharacterSocket);
            }
        }

        private void SetAnimatorArmedType(ECharacterSocket socket)
        {
            switch (socket)
            {
                case ECharacterSocket.Gun:
                    animator.SetFloat("Armed Type", 0.0f);
                    break;
                case ECharacterSocket.Grenade:
                    animator.SetFloat("Armed Type", 1.0f);
                    break;
            };
        }

        private bool isArmed = false;
        private bool isArmedCompleted = false;
        private bool isGrenadeArmedComplete = false;

        public Animator animator;
        public UnityEngine.CharacterController unityCharacterController;
        public CharacterController characterController;
        public Transform cameraPivot;
        public Rigidbody[] ragdollRigidbodies;

        public WeaponBase gunWeapon;
        public WeaponBase grenadeWeapon;
        public Transform weaponSocket;
        public Transform weaponHolder;
        public Transform aimingPoint;
        ECharacterSocket eCurrentCharacterSocket = ECharacterSocket.None;

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
        private float crouchBlend;

        private float targetSpeed;
        private float targetHorizontal;
        private float targetVertical;

        private bool isReloading = false;

        private float aimingRigWeightBlend;
        private float lefthandRigWeightBlend;

        public float rollSpeed = 4.0f;
        private float rollTime;
        public AnimationCurve rollSpeedCurve;

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

        public bool IsZip
        {
            get => isZip;
            set => isZip = value;
        }

        [field : SerializeField] private bool isSprint = true;
        private bool isAutoRunMode = false;
        private bool isWalk = false;
        private bool isRolling = false;
        private bool isZip = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            unityCharacterController = GetComponent<UnityEngine.CharacterController>();
            characterController = GetComponent<CharacterController>();
            ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            SetRagdollActive(false);
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
        }

        private void Update()
        {
            armedBlend = Mathf.Lerp(armedBlend, IsArmed ? 1f : 0f, Time.deltaTime * 10f);
            speedBlend = Mathf.Lerp(speedBlend, targetSpeed, Time.deltaTime * 10f);
            horizontal = Mathf.Lerp(horizontal, targetHorizontal, Time.deltaTime * 10f);
            vertical = Mathf.Lerp(vertical, targetVertical, Time.deltaTime * 10f);
            crouchBlend = Mathf.Lerp(crouchBlend, isCrouch ? 1f : 0f, Time.deltaTime * 10.0f);

            animator.SetFloat("Armed", armedBlend);
            animator.SetFloat("Speed", speedBlend);
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
            animator.SetFloat("Crouch", crouchBlend);

            if (isRolling)
                StartRoll();
        }

        private void LateUpdate()
        {
            aimingRigWeightBlend = Mathf.Lerp(aimingRigWeightBlend, (isArmedCompleted && !isRolling ) || isGrenadeArmedComplete ? 1f : 0f, Time.deltaTime * 10f);
            aimingRig.weight = aimingRigWeightBlend;

            lefthandRigWeightBlend = Mathf.Lerp(lefthandRigWeightBlend, isArmedCompleted && !isReloading && !isRolling && !isGrenadeArmedComplete ? 1f : 0f, Time.deltaTime * 10f);
            lefthandRig.weight = lefthandRigWeightBlend;
        }

        private float targetRotation = 0f;

        public void Move(Vector2 input, float yAxisAngle)
        {
            if (isRolling)
                return;

            if (isZip)
            {
                animator.SetFloat("Magnitude", 0.0f);
                return;
            }

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

        private bool isCrouch = false;
        [SerializeField]
        Vector3 crouchOffset;

        public void Crouch()
        {
            // 카메라 위치를 조금 낮춥시다
            if (!isCrouch)
            {
                CameraSystem.Instance.SetCrouchOffSet(crouchOffset);
                animator.SetFloat("Crouch", 1.0f);
            }
            else
            {
                CameraSystem.Instance.SetCrouchOffSet(Vector3.zero);
                animator.SetFloat("Crouch", 0.0f);
            }

            isCrouch = !isCrouch;
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
                viewForward.y = 0.0f;

                float dotResult = Vector3.Dot(viewForward, direction);
                // 내적값이 음수가 나오면 forward를 카메라 정면 방향으로 변경
                // targetPoint와 플레이어의 거리에 따라 예외처리가 필요할지..?
                if (dotResult < 0.9)
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, viewForward, Time.deltaTime * 10f));
                    return false;
                }

                transform.rotation = Quaternion.LookRotation(Vector3.Lerp(transform.forward, viewForward, Time.deltaTime * 10f));
            }

            return true;
        }

        public void Shoot()
        {
            if (isRolling)
                return;

            if (IsArmed && isArmedCompleted)
            {
                bool isFireSuccess = gunWeapon.Fire();
                if (!isFireSuccess && gunWeapon.CurrentAmmo <= 0)
                {
                    Reload();
                    characterController.PauseRecoil();
                    return;
                }

                if (isFireSuccess)
                    characterController.AddRecoil();
            }
        }

        private void CheckRecoilSystem()
        {
            
        }

        public void MeleeAttack()
        {

        }

        private bool isThrowReady = false;
        public void ThrowReady()
        {
            if (isThrowReady)
            {
                animator.SetFloat("Armed Type", 0.0f);
                isThrowReady = false;
                isGrenadeArmedComplete = false;
                return;
            }

            isThrowReady = grenadeWeapon.ThrowReady();
            animator.SetFloat("Armed Type", 1.0f);
        }

        public void Throw()
        {
            grenadeWeapon.Throw();
            // 던졌으면 Armed 해제 Animator 적용 복구

            // 조건이 추가적으로 더 붙을듯 TODO : 폭탄이 없다면? 0.0f 폭탄이 아직도 소지중이라면 1.0f
            animator.SetFloat("Armed Type", 0.0f); // 임시로 일단 끄는 식으로
            animator.SetTrigger("Throw Trigger");

            // 임시 Animator로 관리 할 필요 있음
            isArmed = false;
            isArmedCompleted = false;
            isGrenadeArmedComplete = false;
        }

        public void ShootFinished()
        {
            characterController.PauseRecoil();
        }

        public void Reload()
        {
            if (!isReloading && gunWeapon.CurrentAmmo != gunWeapon.clipSize)
            {
                isReloading = true;
                animator.SetTrigger("Reload Trigger");
            }
        }

        public void SetReloadComplete()
        {
            gunWeapon.Reload();
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
                gunWeapon.transform.SetParent(weaponHolder);
                gunWeapon.transform.localPosition = offsetPosition;
                gunWeapon.transform.localRotation = Quaternion.Euler(offsetRotation);
            }
            else
            {
                gunWeapon.transform.SetParent(weaponSocket);
                gunWeapon.transform.localPosition = Vector3.zero;
                gunWeapon.transform.localRotation = Quaternion.identity;
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

        public void SetGrenadeArmedComplete(int flag)
        {
            isArmedCompleted = flag > 0;
            isGrenadeArmedComplete = flag > 0;
        }
    }
}