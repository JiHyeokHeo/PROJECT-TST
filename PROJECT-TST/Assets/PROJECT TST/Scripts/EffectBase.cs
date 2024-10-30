using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

namespace TST
{
    public class EffectBase : MonoBehaviour
    {
        // 약간의 오프셋 체크 용 + lifeTime 조절로 Base가 있으면 좋을듯 싶어서 만들었습니다
        // ProjectileBase Collision 쪽 관련.
        // 총알이 벽에 닿을 시 총알 자국 남는 거 방향벡터 관련된 공부중..(Collision ContactPoint)
        public Vector3 offSet = Vector3.zero;
        private float lifeTime = 2.0f;

        public void Activate(Vector3 pos, Quaternion rotation)
        {
            EffectManager.Instance.SpawnEffect(this.gameObject, pos + offSet, rotation);
        }

        public void Activate(Vector3 pos, Vector3 rotation)
        {
            Activate(pos, Quaternion.Euler(rotation));
        }

        // public 으로 열지 말지 고민 Effect매니저에서 처리하는게 옳을지 흠..
        private void ResetEffectData()
        {
            lifeTime = 2.0f;
        }

        public bool UpdateEffectBase(float deltaTime)
        {
            lifeTime -= deltaTime;
            if (lifeTime <= 0)
            {
                ResetEffectData();
                return true;
            }

            return false;
        }
    }
}
