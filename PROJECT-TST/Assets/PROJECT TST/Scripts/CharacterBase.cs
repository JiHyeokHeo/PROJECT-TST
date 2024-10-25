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
        public float rotateSpeed = 5f;

        private float horizontal;
        private float vertical;
        private float speedBlend;
        private float armedBlend;

        private float targetHorizontal;
        private float targetVertical;

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
            horizontal = Mathf.Lerp(horizontal, targetHorizontal, Time.deltaTime * 10f);
            vertical = Mathf.Lerp(vertical, targetVertical, Time.deltaTime * 10f);
            armedBlend = Mathf.Lerp(armedBlend, IsArmed ? 1f : 0f, Time.deltaTime * 10f);

            animator.SetFloat("Armed", armedBlend);
            animator.SetFloat("Speed", speedBlend);
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        }

        public void Move(Vector2 input)
        {
            if (input.magnitude > 0f)
            {
                speedBlend = 1f;
                targetHorizontal = input.x;
                targetVertical = input.y;

                Vector3 movement = (transform.forward * input.y + transform.right * input.x) * moveSpeed * Time.deltaTime;
                unityCharacterController.Move(movement);
            }
            else
            {
                speedBlend = 0f;
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