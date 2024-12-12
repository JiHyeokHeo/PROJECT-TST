using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public abstract class AIStateBase
    {
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
