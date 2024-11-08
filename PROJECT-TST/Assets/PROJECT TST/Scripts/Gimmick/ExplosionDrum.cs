using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class ExplosionDrum : MonoBehaviour, IDamage
    {
        public float drumHP = 100;
        public GameObject explosionEffect;

        private void Awake()
        {
            explosionEffect.gameObject.SetActive(false);
        }

        public void ApplyDamage(float damage)
        {
            drumHP -= damage;

            if (drumHP <= 0)
            {
                // TODO : Add explosion effect
                explosionEffect.gameObject.SetActive(true);
                explosionEffect.transform.SetParent(null);
                Destroy(explosionEffect.gameObject, 3f);

                Destroy(gameObject);
            }
        }
    }
}
