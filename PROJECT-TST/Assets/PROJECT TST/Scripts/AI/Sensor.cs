using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class Sensor : MonoBehaviour
    {
        public BoxCollider dectectCollider;
        public BoxCollider attackCollider;

        [SerializeField]
        private float detectRange = 10.0f;

        [SerializeField]
        private float attackRange = 5.0f;
        // Start is called before the first frame update
        void Start()
        {
            dectectCollider.size = new Vector3(detectRange, 2.0f, detectRange);
            attackCollider.size = new Vector3(attackRange, 2.0f, attackRange);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

    }
}
