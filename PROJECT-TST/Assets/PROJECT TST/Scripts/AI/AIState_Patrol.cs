using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TST
{
    [Serializable]
    public class AIState_Patrol : AIStateBase
    {
        private CharacterBase linkedCharacter;

        public float patrolInterval = 4.0f; // 순찰 시간 간격
        private float lastPatrolTime;

        public float patrolRange = 20.0f; // 순찰 범위
        private Vector3 targetPosition;

        public AIState_Patrol(CharacterBase character, NavMeshAgent agent)
        {
            linkedCharacter = character;
            base.agent = agent;
        }

        public override void Enter()
        {
            UpdatePatrolDestination();
            //linkedCharacter.AIMove(true);
        }

        public override void Exit()
        {
            // Exit 시 필요한 로직이 있을 경우 추가
            // 나갈때 한번만 애니메이터 끄는 용으로
            agent.ResetPath();
            //linkedCharacter.AIMove(false);
        }

        public override void Update()
        {
            if (Time.time - lastPatrolTime > patrolInterval)
            {
                UpdatePatrolDestination();
                //linkedCharacter.AIMove(true);
            }

            if (agent != null && !agent.pathPending && agent.remainingDistance < 0.01f)
            {
                // 움직이면 1.0f // 안움직이면 0.0f
                //linkedCharacter.AIMove(false);
            }
        }

        private void UpdatePatrolDestination()
        {
            lastPatrolTime = Time.time;

            // 현재 캐릭터 위치를 기준으로 랜덤한 위치 계산
            Vector3 aiPosition = linkedCharacter.transform.position;
            float randX = UnityEngine.Random.Range(-patrolRange, patrolRange);
            float randZ = UnityEngine.Random.Range(-patrolRange, patrolRange);

            targetPosition = new Vector3(aiPosition.x + randX, aiPosition.y, aiPosition.z + randZ);

            // 목표 위치 설정
            if (agent != null)
            {
                agent.SetDestination(targetPosition);
            }
        }

        public override void SetTarget(GameObject target)
        {
            
        }
    }
}
