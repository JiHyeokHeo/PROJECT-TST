using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class AttackCollider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // 공격범위 안에서는 싸움 ㄱㄱ
            if (other.TryGetComponent(out IDetect detectInterface))
            {
                detectInterface.CombatDetect(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // 공격범위 탈출하면 다시 그냥 검색 범위안에 들어온 것
            if (other.TryGetComponent(out IDetect detectInterface))
            {
                detectInterface.Detect(other.gameObject);
            }
        }
    }
}
