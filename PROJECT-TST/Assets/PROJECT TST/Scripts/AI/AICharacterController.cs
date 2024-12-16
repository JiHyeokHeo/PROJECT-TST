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

            // Snesor 스크립트 안에 있다면 Combat 스테이트로 진입
            characterBase.OnDetect += (target) => SetState(new AIState_Combat(characterBase));
            characterBase.OnDetect += (target) => SetTarget(target);

            // Sensor 스크립트 탐지 범위 바깥으로 빠지면 Idle 상태로 진입
            characterBase.OnIdle += (target) => SetState(new AIState_Idle(characterBase));
            characterBase.OnIdle += (target) => SetTarget(target);
        }

        private void Update()
        {
            currentState.Update();
        }

        public void SetState(AIStateBase newState)
        {
            if (currentState == newState)
                return;

            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void SetTarget(GameObject target)
        {
            currentState.SetTarget(target);
        }
    }
}
