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
            // 전투 상태에서 빠져나가면 처리할 작업.

        }

        public override void Update()
        {
            // Physics. Overlap => 주변에 적 캐릭터가 있나...?

        }
    }
}
