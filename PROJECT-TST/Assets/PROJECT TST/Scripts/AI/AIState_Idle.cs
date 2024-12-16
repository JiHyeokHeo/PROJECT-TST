using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TST
{
    public class AIState_Idle : AIStateBase
    {
        private CharacterBase linkedCharacter;
        public Vector3 startPosition;

        private Vector3 targetPosition;

        public AIState_Idle(CharacterBase characterBase)
        {
            linkedCharacter = characterBase;
        }

        public override void Enter()
        {
            // 진입했으면 다시 원 포지션으로 돌아가도록
            targetPosition = startPosition;
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            // 위치 이동
            Vector3 direction = targetPosition - linkedCharacter.transform.position;
            //linkedCharacter.Move()



        }

        public override void SetTarget(GameObject target)
        {

        }
    }
}
