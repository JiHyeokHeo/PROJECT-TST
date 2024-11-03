using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace TST
{
    public class ProjectileBase : MonoBehaviour
    {
        public Rigidbody rigid;

        public float bulletForce;
        public float lifeTime;

        public void SetInfo()
        {
            rigid.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
            Destroy(gameObject, lifeTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            GameObject effect = null;
            if (collision.collider.material.name.Contains("Metal"))
            {
                // Metal Effect Spawn
                effect = EffectManager.Instance.SpawnEffect(EffectType.Impact_Metal);
            }
            else if (collision.collider.material.name.Contains("Dirt"))
            {
                // Dirt Effect Spawn
                effect = EffectManager.Instance.SpawnEffect(EffectType.Impact_Brick);
            }
            else
            {
                // Default Effect Spawn
                effect = EffectManager.Instance.SpawnEffect(EffectType.Impact_Dirt);
            }

            effect.transform.SetPositionAndRotation(collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));

            Destroy(gameObject);
        }
    }
}
