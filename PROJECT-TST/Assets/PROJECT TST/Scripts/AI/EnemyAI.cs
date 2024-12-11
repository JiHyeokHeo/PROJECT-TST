using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public enum EEnemyAIState
    {
        Idle,
        Move,
        Attack,
    }

    public class EnemyAI : MonoBehaviour
    {
        public EEnemyAIState EnemyAIState
        {
            get => enemyAIState;
            set
            {
                if (enemyAIState == value) return;
            }
        }

        public Animator enemyAnimator;
        public Rigidbody rigidBody;
        private EEnemyAIState enemyAIState;

        public GameObject target;
        public float detectRange = 50.0f;
        public float attackRange = 10.0f;

        private float IdleBlend;
        private float speedBlend;
        private float vertical;
        void Start()
        {
        
        }

        void Update()
        {
            // 동작용
            switch (EnemyAIState)
            {
                case EEnemyAIState.Idle:
                    Idle();
                    break;
                case EEnemyAIState.Move:
                    Move();
                    break;
                case EEnemyAIState.Attack:
                    Attack();
                    break;
            }

            // 애니메이터용
            switch (EnemyAIState)
            {
                case EEnemyAIState.Idle:
                    IdleAnimator();
                    break;
                case EEnemyAIState.Move:
                    MoveAnimator();
                    break;
                case EEnemyAIState.Attack:
                    AttackAnimator();
                    break;
            }

            //enemyAnimator.SetFloat("Armed", armedBlend);

            enemyAnimator.SetFloat("Speed", speedBlend);
            enemyAnimator.SetFloat("Idle Blend", IdleBlend);
            //enemyAnimator.SetFloat("Horizontal", horizontal);
            enemyAnimator.SetFloat("Vertical", vertical);
            //enemyAnimator.SetFloat("Crouch", crouchBlend);
        }
  
        private void Idle()
        {
            // Idle 상태에서 일단 고민을 한다 공격을 할지 뭘 할지 일단은 그냥 움직이는 걸로만 하는거로 뭐 귀찮으니 타겟 그냥 플레이어로 고정 박읍시다.
            // + 타겟과 ai 거리를 지속적으로 측정 해주는 식으로 한다. // 하지만 매 프레임마다 모든 ai들이 다 측정하면 상당히 비효율적일듯. 
            // 존 시스템 같은 거로(맵을 100x100x100 3Dimensional 나눠서 작업한다거나) ai연산 분할 작업을 하는게 좋아보임 (자료를 좀 찾아볼까나 이런 기술이 있겠지..?)
            if (Vector3.Distance(target.transform.position, transform.position) < detectRange)
                enemyAIState = EEnemyAIState.Move;

        }

        public float moveSpeed = 0.5f;
        private float targetSpeed;
        [field:SerializeField] private Quaternion targetRotation;
        private void Move()
        {
            if (Vector3.Distance(target.transform.position, transform.position) >= detectRange)
            {
                enemyAIState = EEnemyAIState.Idle;
                return;
            }

            Vector3 direction = (target.transform.position - transform.position).normalized;

            // 움직여봅시다이
            targetSpeed = moveSpeed * 3.0f;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // 머리 돌려주고
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);

            // 
            if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
            {
                enemyAIState = EEnemyAIState.Attack;
                return;
            }
        }

        private void Attack()
        {
            if (Vector3.Distance(target.transform.position, transform.position) >= attackRange)
            {
                enemyAIState = EEnemyAIState.Move;
                return;
            }

            if (Vector3.Distance(target.transform.position, transform.position) >= detectRange)
            {
                enemyAIState = EEnemyAIState.Idle;
                return;
            }

            Vector3 direction = (target.transform.position - transform.position).normalized;

            transform.position = transform.position;
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);

            Debug.Log("AI 공격중 !");
        }

        private void IdleAnimator()
        {
            IdleBlend = 0;
            // Idle 상태일때 다 밀어버리기
            targetSpeed = 0f;
        }

        private void MoveAnimator()
        {
            IdleBlend = 1;  
            speedBlend = Mathf.Lerp(speedBlend, targetSpeed, Time.deltaTime * 10.0f);
        }

        private void AttackAnimator()
        {

        }

    }
}
