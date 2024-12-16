using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class AIState_Patrol : AIStateBase
    {
        private CharacterBase linkedCharacter;
        private GameObject aiTarget;

        public AIState_Patrol(CharacterBase character)
        {
            linkedCharacter = character;
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void SetTarget(GameObject target)
        {
            aiTarget = target;
        }
    }
}
