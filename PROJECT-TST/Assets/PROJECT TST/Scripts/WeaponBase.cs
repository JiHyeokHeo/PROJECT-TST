using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class WeaponBase : MonoBehaviour
    {
        public Transform firePoint; // �Ѿ� �߻� ��ġ
        public float fireRate; // ���� �ӵ�
        public int clipSize; // źâ ũ��[1źâ:�Ѿ� ����]

        private int currentAmmo; // ���� źâ�� ���� �Ѿ� ��
        private float lastFireTime; // ������ �߻� �ð�


        public Rigidbody bulletPrefab;
        public float bulletSpeed;
        public float bulletLifeTime = 3f;

        private void Awake()
        {
            currentAmmo = clipSize;
        }

        public void Fire()
        {
            if (currentAmmo > 0 && Time.time - lastFireTime >= fireRate)
            {
                lastFireTime = Time.time;
                //currentAmmo--;

                // TODO : ���� �Ѿ� ����/�߻�
                Rigidbody newBullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
                newBullet.gameObject.SetActive(true);
                newBullet.AddForce(firePoint.transform.forward * bulletSpeed, ForceMode.Impulse);
                Destroy(newBullet.gameObject, bulletLifeTime);
            }
        }

        public void Reload()
        {
            currentAmmo = clipSize;
        }
    }
}
