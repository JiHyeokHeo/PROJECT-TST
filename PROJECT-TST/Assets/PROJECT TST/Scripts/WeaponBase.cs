using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class WeaponBase : MonoBehaviour
    {
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



        private void Awake()
        {
            currentAmmo = clipSize;
        }

        private void Update()
        {
            
        }

        public void Fire()
        {
            if (gameObject.activeSelf == false)
                return;

            if (currentAmmo > 0 && Time.time - lastFireTime >= fireRate)
            {
                lastFireTime = Time.time;
                currentAmmo--;

                // TODO : 실제 총알 복제/발사
                Rigidbody newBullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
                newBullet.gameObject.SetActive(true);
                newBullet.AddForce(firePoint.transform.forward * bulletSpeed, ForceMode.Impulse);
                Destroy(newBullet.gameObject, bulletLifeTime);

                EffectManager.Instance.effects[0].Activate(firePoint.transform.position, firePoint.transform.rotation);
            }
        }

        public void Reload()
        {
            if (gameObject.activeSelf == false)
                return;

            currentAmmo = clipSize;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null) 
                return;

            // 접촉한 정보가 있다면 Effect 발사 & 총알 삭제
            // 임시로 0번은 MuzzleFlash, 1번은 BrickImpact로 설정
            EffectManager.Instance.effects[1].Activate(collision.transform.position, collision.transform.rotation);
            
            Destroy(this.gameObject);
        }
    }
}
