using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace TST
{
    public class AICharacterController : MonoBehaviour
    {
        [SerializeReference]
        public AIStateBase currentState;

        private CharacterBase characterBase;

        
        private Dictionary<string, AIStateBase> states = new Dictionary<string, AIStateBase>();
        private void Awake()
        {
            characterBase = GetComponent<CharacterBase>();
        }

        private void Start()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();

            if (agent == null)
            {
                Debug.Log("Agent Missing");
                return;
            }

            // 상태 객체를 미리 생성해 둠
            currentState = new AIState_Patrol(characterBase, agent);
            characterBase.OnDamaged += (target) => SetState(new AIState_Combat(characterBase, agent));
            characterBase.OnDamaged += (target) => SetTarget(target);

            // Sensor 스크립트 안에 있다면 Combat 스테이트로 진입
            // 공격 범위 밖에 있다가 다시 탐지 범위에 들어가게 되도 공격모드 진입 
            //characterBase.OnDetect += (target) => SetState<GameObject>(new AIState_Move(characterBase, agent),
            //    beforeEnterEvent: (t) => SetTarget(target));

            characterBase.OnDetect += (target) => SetState(new AIState_Move(characterBase, agent));
            characterBase.OnDetect += (target) => SetTarget(target);
                

            characterBase.OnCombatDetect += (target) => SetState(new AIState_Combat(characterBase, agent));
            characterBase.OnCombatDetect += (target) => SetTarget(target); 

            // Sensor 스크립트 탐지 범위 바깥으로 빠지면 Idle 상태로 진입
            characterBase.OnIdle += (target) => SetState(new AIState_Idle(characterBase, agent));

            // 결론 처음엔 Patrol 진입 하지만 센서로 인해 Combat or Idle 상태로 진입 // Idle 상태에서 특정 시간이 되면 다시 Patrol 진입
        }

        private void Update()
        {
            currentState.Update();
            Debug.Log($"{currentState}");
        }

        public void SetState(AIStateBase newState)
        {
            SetState<GameObject>(newState);
        }

        public void SetState<T>(AIStateBase newState, Action<T> beforeEnterEvent = null, T beforeEnterParam = default)
        {
            if (currentState == newState)
                return;

            currentState.Exit();
            currentState = newState;
            beforeEnterEvent?.Invoke(beforeEnterParam);
            currentState.Enter();
        }

        public void SetTarget(GameObject target)
        {
            currentState.SetTarget(target);
        }
    }
}
