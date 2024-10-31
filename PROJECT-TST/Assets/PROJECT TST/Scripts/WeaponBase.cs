using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class WeaponBase : MonoBehaviour
    {
        public int CurrentAmmo => currentAmmo;

        public Transform firePoint; // 총알 발사 위치
        public float fireRate; // 연사 속도
        public int clipSize; // 탄창 크기[1탄창:총알 갯수]

        private int currentAmmo; // 현재 탄창에 남은 총알 수
        private float lastFireTime; // 마지막 발사 시간


        public Rigidbody bulletPrefab;
        public float bulletSpeed;
        public float bulletLifeTime = 3f;

        private void Awake()
        {
            currentAmmo = clipSize;
        }

        public bool Fire()
        {
            if (currentAmmo > 0 && Time.time - lastFireTime >= fireRate)
            {
                lastFireTime = Time.time;
                currentAmmo--;

                // TODO : 실제 총알 복제/발사
                Rigidbody newBullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
                newBullet.gameObject.SetActive(true);

                Destroy(newBullet.gameObject, bulletLifeTime);

                var effect = EffectManager.Instance.SpawnEffect(EffectType.Muzzle_6);
                effect.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

                return true;
            }

            return false;
        }

        public void Reload()
        {
            currentAmmo = clipSize;
        }
    }
}
