using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class AIState_Patrol : AIStateBase
    {
        private CharacterBase linkedCharacter;

        public AIState_Patrol(CharacterBase character) 
        { 
            linkedCharacter = character;
        }


        public override void Enter()
        {
            // 순찰 상태 진입에 따른 초기화 작업.
            linkedCharacter.IsArmed = false;
        }

        public override void Exit()
        {
            // 순찰 상태에서 빠져나가면 처리할 작업.

        }

        public override void Update()
        {
            
        }
    }
}
