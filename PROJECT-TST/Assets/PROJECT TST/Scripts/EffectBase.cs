using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

namespace TST
{
    public class EffectBase : MonoBehaviour
    {
        public float EffectPooledTime = 2.0f;

        IEnumerator PushEffectPoolCoroutine()
        {
            yield return new WaitForSeconds(EffectPooledTime);
            DeActivate();
            StopCoroutine(PushEffectPoolCoroutine());
        }
  
        public void Activate(Vector3 pos, Quaternion rotation)
        {
            // 성공적으로 이펙트를 Pop 해왔다면 다시 
            GameObject popObject = EffectManager.Instance.SpawnEffect(this.gameObject, pos, rotation);
            if (popObject != null && popObject.activeSelf)
                popObject.GetComponent<EffectBase>().StartPushEffectPoolCoroutine();
        }

        public void Activate(Vector3 pos, Vector3 rotation)
        {
            Activate(pos, Quaternion.Euler(rotation));
        }

        private void DeActivate()
        {
            EffectManager.Instance.PushEffectToPool(this.gameObject);
        }

        private void StartPushEffectPoolCoroutine()
        {
            StartCoroutine(PushEffectPoolCoroutine());
        }
    }
}
