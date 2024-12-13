using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class AIState_Combat : AIStateBase
    {
        private CharacterBase linkedCharacter;

        public AIState_Combat(CharacterBase character)
        {
            linkedCharacter = character;
        }

        public override void Enter()
        {   
            // 전투 상태 진입에 따른 초기화 작업.
            linkedCharacter.IsArmed = true;
        }

        public override void Exit()
        {
            // 전투 상태 빠져나갈시 작업.

            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            // Physics, Overlap 주변에 적이 있나 없나 확인용
            throw new System.NotImplementedException();
        }
    }
}
