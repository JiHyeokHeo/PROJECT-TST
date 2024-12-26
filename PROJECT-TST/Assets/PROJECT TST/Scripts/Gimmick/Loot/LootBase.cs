using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public abstract class LootBase : MonoBehaviour, IInteractable
    {
        public string Message => message;

        protected float interactRange = 5.0f;
        protected float lootTime = 2.0f;
        private float timeElapsed = 0.0f;

        protected string message;
        private event Action<bool> lootSucceed;

        public void Update()
        {
            lootSuccessCheck();
        }

        private void lootSuccessCheck()
        {
            if (lootSucceed == null)
                return;

            timeElapsed += Time.deltaTime;

            if (timeElapsed > lootTime)
            {
                timeElapsed = 0.0f;
                lootSucceed?.Invoke(true);
                lootSucceed = null;
            }
        }

        public void Interact(GameObject go)
        {
            if (go.TryGetComponent(out CharacterBase playerComponent) == false)
                return;

            Vector3 itemPos = this.gameObject.transform.position;
            Vector3 playerPos = playerComponent.transform.position;

            float sqrDistMagnitude = Vector3.SqrMagnitude(itemPos - playerPos);
            if (sqrDistMagnitude > interactRange * interactRange)
                return;

            
            SetInitialize();
            SetInteractLootType(playerComponent);
            lootSucceed += playerComponent.SetLootisSucceed;

            playerComponent.SetInteractAnimation(EInteractionType.Looting);
            Debug.Log($"<b><color=red> {Message}!</color></b>");
        }

        public abstract void SetInteractLootType(CharacterBase playerComponent);
        public abstract void SetInitialize(); // 기본적인 세팅 
    }
}
