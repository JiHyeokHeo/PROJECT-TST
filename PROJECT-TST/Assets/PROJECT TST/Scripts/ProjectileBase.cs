using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class ProjectileBase : MonoBehaviour
    {
        public void SetInfo()
        {
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null)
                return;

            ContactPoint contactPoint = collision.contacts[0];
            // 접촉한 정보가 있다면 Effect 발사 & 총알 삭제
            // 임시로 0번은 MuzzleFlash, 1번은 BrickImpact로 설정

            // 임시
            gameObject.SetActive(false);
        }
    }
}
