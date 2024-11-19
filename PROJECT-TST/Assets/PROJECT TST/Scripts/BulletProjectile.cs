using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class BulletProjectile : ProjectileBase
    {
        protected override void Init()
        {
            rigid.AddForce(transform.forward * moveForce, ForceMode.Impulse);
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
            else if (collision.collider.material.name.Contains("Brick"))
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

            if (collision.transform.root.TryGetComponent(out IDamage damageInterface))
            {
                damageInterface.ApplyDamage(10);
            }

            Destroy(gameObject);
        }
    }
}
