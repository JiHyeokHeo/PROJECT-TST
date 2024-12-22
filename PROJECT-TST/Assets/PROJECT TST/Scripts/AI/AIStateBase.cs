using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TST
{
    [Serializable]
    public abstract class AIStateBase 
    {
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

        public abstract void SetTarget(GameObject target);
    }
}
