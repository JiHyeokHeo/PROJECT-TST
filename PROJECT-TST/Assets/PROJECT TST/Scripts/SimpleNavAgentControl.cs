using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TST
{
    public class SimpleNavAgentControl : MonoBehaviour
    {
        public NavMeshAgent agent;
        public Transform targetPoint;

        private void Update()
        {
            agent.SetDestination(targetPoint.position);            
        }
    }
}
