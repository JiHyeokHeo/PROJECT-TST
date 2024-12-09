using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class Target : MonoBehaviour, IDamage
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _size = 10;
        [SerializeField] private float _speed = 10;
        public Rigidbody Rb => _rb;

        void Update()
        {
            float x = Mathf.Sin(Time.time) * 20.0f;
            Vector3 newPos = transform.position;
            newPos.x = x;
            transform.position = newPos;


            //var dir = new Vector3(Mathf.Cos(Time.time * _speed) * _size, Mathf.Sin(Time.time * _speed) * _size);

            //_rb.velocity = dir;
        }
        
        public void ApplyDamage(float damage)
        {
            if (damage > 100)
            {
                Destroy(gameObject);
            }
        }
    }
}
