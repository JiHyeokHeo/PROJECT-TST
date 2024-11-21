using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class WeaponBase : MonoBehaviour
    {
        // 클래스 분할이 필요할까요?
        #region Bullet
        public Transform firePoint; // 총알 발사 위치
        public float fireRate = 0.1f; // 연사 속도
        public int clipSize = 10; // 탄창 크기[1탄창:총알 갯수]

        public int CurrentAmmo
        {
            get => currentAmmo;
            private set { }
        }

        private int currentAmmo; // 현재 탄창에 남은 총알 수
        private float lastFireTime; // 마지막 발사 시간

        public Rigidbody bulletPrefab;
        public float bulletSpeed;
        public float bulletLifeTime = 3f;
        #endregion

        #region Bomb
        public BombProjectile bombProjectilePrefab;
        public Transform bombHoldPoint;
        public Vector3 offSet;
        #endregion


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

        #region Grenade
        // 수류탄 추후 클래스 분할 리팩토링 필요해보임
        public BombProjectile throwReadyGrenade;
        public bool ThrowReady()
        {
            if (Time.time - lastFireTime >= fireRate)
            {
                lastFireTime = Time.time;
                //currentAmmo--;

                // 수류탄 발사

                Vector3 offset = new Vector3(0, 60, 0); // 추가하고 싶은 오프셋 (X, Y, Z 각도 단위)
                Quaternion originalRotation = bombHoldPoint.transform.rotation; // 원래 회전
                Quaternion offsetRotation = Quaternion.Euler(offset); // 오프셋을 Quaternion으로 변환

                // 원래 회전에 오프셋 적용
                Quaternion finalRotation = originalRotation * offsetRotation;

                throwReadyGrenade = Instantiate(bombProjectilePrefab, bombHoldPoint.transform.position, finalRotation);
                throwReadyGrenade.gameObject.SetActive(true);
                throwReadyGrenade.gameObject.transform.SetParent(bombHoldPoint);
                throwReadyGrenade.ThrowReady();

                return true;
            }

            return false;
        }

        public bool Throw()
        {
            if (throwReadyGrenade == null)
                return false;

            // 앞으로 날라가도록
            throwReadyGrenade.Throw();
            throwReadyGrenade.gameObject.transform.SetParent(null);
            return true;
        }
        #endregion

        
    }
}
