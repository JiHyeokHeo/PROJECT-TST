using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TST.LootAnimation;

namespace TST
{
    public enum ELootType
    {
        None = 0,
        BackPack = 1,
        End,
    }

    public class LootBackPack : MonoBehaviour, IInteractable
    {
        public string Message => "LootBackPack";

        public float interactRange = 5.0f;

        public float lootTime = 2.0f;

        private float timeElapsed = 0.0f;

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

            playerComponent.SetLootType((float)ELootType.BackPack);
            lootSucceed += playerComponent.SetLootisSucceed;

            playerComponent.SetInteractAnimation(EInteractionType.Looting);
            Debug.Log($"<b><color=red> {Message}!</color></b>");
        }
    }
}
