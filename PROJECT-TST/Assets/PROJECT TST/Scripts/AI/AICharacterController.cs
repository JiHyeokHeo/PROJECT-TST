using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TST
{
    public class AICharacterController : MonoBehaviour
    {
        public AIStateBase currentState;

        private CharacterBase characterBase;

        private void Awake()
        {
            characterBase = GetComponent<CharacterBase>();

            
        }

        private void Start()
        {
            currentState = new AIState_Patrol(characterBase);

            characterBase.OnDamaged += () => SetState(new AIState_Combat(characterBase));
        }


        public void SetState(AIStateBase newState)
        {
            if (currentState == newState)
                return;

            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
