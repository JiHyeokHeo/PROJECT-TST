using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace TST
{
    [Serializable]
    public class AIState_Move : AIStateBase
    {
        private CharacterBase linkedCharacter;
        private AICharacterController linkedCharacterController;
        private GameObject aiTarget;

        public AIState_Move(AICharacterController aiController)
        {
            linkedCharacter = aiController.LinkedCharacter;
            linkedCharacterController = aiController;
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {
            // NavMesh 쓰고 있었다면 탈출과 동시에 Path 서칭 취소
        }

        public override void Update()
        {
            if (linkedCharacterController.NavAgent.pathPending == false && linkedCharacterController.NavAgent.remainingDistance < 0.01f)
            {
                FollowTarget();
            }
        }

        public override void SetTarget(GameObject target)
        {
            aiTarget = target;
        }

        public void FollowTarget()
        {
            if (aiTarget == null)
            {
                Debug.Log("ai Target Issue");
                return;
            }

            // 총 빼면서 움직이는거 방지하기 위해서
            if (linkedCharacter.IsArmed == false && linkedCharacter.IsArmedCompleted == true)
            {
                linkedCharacterController.NavAgent.ResetPath();
                return;
            }
            // 반대 버전
            if (linkedCharacter.IsArmed == true && linkedCharacter.IsArmedCompleted == false)
            {
                linkedCharacterController.NavAgent.ResetPath();
                return;
            }

            // 현재 캐릭터 위치를 기준으로 랜덤한 위치 계산
            Vector3 aiPosition = aiTarget.transform.position;

            // 목표 위치 설정
            linkedCharacterController.SetDestination(aiPosition);
        }
    
}
}
