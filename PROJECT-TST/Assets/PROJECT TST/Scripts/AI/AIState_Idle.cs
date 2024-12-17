using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace TST
{
    [Serializable]
    public class AIState_Idle : AIStateBase
    {
        private CharacterBase linkedCharacter;
        private AICharacterController controller;
        private float idleDuration = 5.0f; // 대기 시간
        private float idleStartTime;

        private Vector3 spawnPosition;

        public AIState_Idle(CharacterBase character, NavMeshAgent agent)
        {
            linkedCharacter = character;
            base.agent = agent;
            controller = character.gameObject.GetComponent<AICharacterController>();
        }

        public override void Enter()
        {
            // 현재 위치를 기준으로 복귀 좌표 설정
            spawnPosition = linkedCharacter.transform.position;
            idleStartTime = Time.time;

            if (agent != null)
            {
                agent.SetDestination(spawnPosition);
            }
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            if (agent != null && !agent.pathPending && agent.remainingDistance > 0.01f)
            {
                // 아직 목표 위치로 이동 중이라면 업데이트 종료
                return;
            }

            // 대기 시간이 경과했으면 Patrol 상태로 전환
            if (Time.time - idleStartTime >= idleDuration)
            {
                controller.SetState(new AIState_Patrol(linkedCharacter, agent));
            }
        }

        public override void SetTarget(GameObject target)
        {

        }
    }
}
