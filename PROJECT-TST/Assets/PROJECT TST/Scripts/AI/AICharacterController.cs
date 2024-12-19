using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

namespace TST
{
    public class AICharacterController : MonoBehaviour
    {
        public CharacterBase LinkedCharacter => characterBase;

        [SerializeReference]
        public AIStateBase currentState;

        private CharacterBase characterBase;
        private NavMeshAgent navAgent;
        
        private Dictionary<string, AIStateBase> states = new Dictionary<string, AIStateBase>();

        private void Awake()
        {
            characterBase = GetComponent<CharacterBase>();
            navAgent = GetComponent<NavMeshAgent>();

            navAgent.updatePosition = false;
            navAgent.updateRotation = false;
        }

        private void Start()
        {
            // 상태 객체를 미리 생성해 둠
            currentState = new AIState_Patrol(characterBase, navAgent);
            characterBase.OnDamaged += (target) => SetState(new AIState_Combat(characterBase, navAgent));
            characterBase.OnDamaged += (target) => SetTarget(target);

            // Sensor 스크립트 안에 있다면 Combat 스테이트로 진입
            // 공격 범위 밖에 있다가 다시 탐지 범위에 들어가게 되도 공격모드 진입 
            //characterBase.OnDetect += (target) => SetState<GameObject>(new AIState_Move(characterBase, navAgent),
            //    beforeEnterEvent: (t) => SetTarget(target));

            characterBase.OnDetect += (target) => SetState(new AIState_Move(characterBase, navAgent));
            characterBase.OnDetect += (target) => SetTarget(target);
                

            characterBase.OnCombatDetect += (target) => SetState(new AIState_Combat(characterBase, navAgent));
            characterBase.OnCombatDetect += (target) => SetTarget(target); 

            // Sensor 스크립트 탐지 범위 바깥으로 빠지면 Idle 상태로 진입
            characterBase.OnIdle += (target) => SetState(new AIState_Idle(characterBase, navAgent));

            // 결론 처음엔 Patrol 진입 하지만 센서로 인해 Combat or Idle 상태로 진입 // Idle 상태에서 특정 시간이 되면 다시 Patrol 진입
        }

        private void Update()
        {
            currentState.Update();

            // NavAgent의 다음 위치 값을, 현재 위치로 설정한다.
            navAgent.nextPosition = transform.position;

            if (navAgent.pathStatus == NavMeshPathStatus.PathComplete && RemainingDistance() <= navAgent.stoppingDistance)
            {
                // 도착했을 때
                characterBase.Move(Vector2.zero, transform.eulerAngles.y);
            }
            else // 아직 도착하지 않은 상태
            {                
                if (navAgent.hasPath) // 경로가 있는 경우 => NavAgent가 목적지로 이동중인 경우.
                {
                    Vector3 moveDirection = (navAgent.steeringTarget - transform.position).normalized;
                    Vector2 input = new Vector2(moveDirection.x, moveDirection.z);
                    characterBase.Move(input, 0);
                }
                else // 경로가 없는 경우 => NavAgent가 목적지로 이동중이 아닌 경우.
                {
                    characterBase.Move(Vector2.zero, 0);
                }
            }

            Debug.Log($"{currentState}");
        }

        public float RemainingDistance()
        {
            if (!navAgent.isOnNavMesh)
                return float.MaxValue;
            if (navAgent.pathPending)
                return float.MaxValue;

            return navAgent.remainingDistance;
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

        public void SetDestination(Vector3 destination)
        {
            navAgent.SetDestination(destination);
        }
    }
}
