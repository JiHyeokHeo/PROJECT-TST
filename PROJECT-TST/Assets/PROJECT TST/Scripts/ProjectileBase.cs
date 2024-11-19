using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TST
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        public Rigidbody rigid;

        public float bulletForce;
        public float lifeTime;

        public void Start()
        {
            Init();
        }

        protected abstract void Init();
        
    }
}
