using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TST
{
    public class EffectBase : MonoBehaviour
    {
        public IObjectPool<EffectBase> ObjectPool { set => objectPool = value; }

        private IObjectPool<EffectBase> objectPool;

        private void Update()
        {
            
        }
    }
}
