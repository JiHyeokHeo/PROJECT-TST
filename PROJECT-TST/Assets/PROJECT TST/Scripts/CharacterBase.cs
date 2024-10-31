using System.Collections;
using System.Collections.Generic;
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
                SetEquipWeapon(isArmed);
            }
        }
        private bool isArmed = false;
        private bool isArmedCompleted = false;

        public bool IsWalk
        {
            get => isWalk;
            set => isWalk = value;
        }
        private bool isWalk = false;

        public Animator animator;
        public UnityEngine.CharacterController unityCharacterController;
        public WeaponBase weapon;
        public Transform weaponSocket;
        public Transform weaponHolder;
        public Transform aimingPoint;


        public RigBuilder rigBuilder;
        public Rig aimingRig;
        public Rig lefthandRig;

        public Vector3 offsetPosition;
        public Vector3 offsetRotation;

        public float moveSpeed = 2f;
        public float rotateSpeed = 5f;

        private float horizontal;
        private float vertical;
        private float speedBlend;
        private float armedBlend;

        private float targetHorizontal;
        private float targetVertical;

        private bool isReloading = false;

        private float aimingRigWeightBlend;
        private float lefthandRigWeightBlend;



        private void Awake()
        {
            animator = GetComponent<Animator>();
            unityCharacterController = GetComponent<UnityEngine.CharacterController>();
        }

        private void Start()
        {
            aimingRig.weight = 0f;
            lefthandRig.weight = 0f;
            rigBuilder.Build();
        }

        private void Update()
        {
            horizontal = Mathf.Lerp(horizontal, targetHorizontal, Time.deltaTime * 10f);
            vertical = Mathf.Lerp(vertical, targetVertical, Time.deltaTime * 10f);
            armedBlend = Mathf.Lerp(armedBlend, IsArmed ? 1f : 0f, Time.deltaTime * 10f);
            speedBlend = Mathf.Lerp(speedBlend, IsWalk ? 0f : 1f, Time.deltaTime * 10f);

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

        public void Move(Vector2 input)
        {
            if (input.magnitude > 0f)
            {
                targetHorizontal = input.x;
                targetVertical = input.y;

                Vector3 movement = (transform.forward * input.y + transform.right * input.x) * moveSpeed * Time.deltaTime;
                unityCharacterController.Move(movement);
            }
            else
            {
                targetHorizontal = 0f;
                targetVertical = 0f;
            }

            animator.SetFloat("Magnitude", input.magnitude);
        }

        public void Rotate(float rotation)
        {
            transform.Rotate(Vector3.up * rotation * rotateSpeed * Time.deltaTime);
        }

        public void Shoot()
        {
            if (IsArmed && isArmedCompleted)
            {
                bool isFireSuccess = weapon.Fire();
                if (!isFireSuccess && weapon.CurrentAmmo <= 0)
                {
                    Reload();
                }
            }
        }

        public void Reload()
        {
            // # 재장전 애니메이션 Trigger 호출
            // TODO : 이미 재장전을 하고 있었다면? 재장전을 하지 않도록 예외처리하자.
            if (!isReloading)
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

        public void SetArmedComplete(int flag)
        {
            isArmedCompleted = flag > 0;
        }
    }
}