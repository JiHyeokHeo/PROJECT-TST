using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TST
{
    public class LootAnimation : MonoBehaviour, IInteractable
    {
        public enum ELootState
        {
            Start,
            Keep,
            Loop,
            End,
            None,
        }

        public ELootState LootState
        {
            get => lootState;
            set
            {
                lootState = value;

                switch (lootState)
                {
                    case ELootState.Start:
                        playerComponent.SetLootInteractAnimation(ELootState.Start);
                        break;
                    case ELootState.Keep:
                        playerComponent.SetLootInteractAnimation(ELootState.Keep);
                        break;
                    case ELootState.Loop:
                        playerComponent.SetLootInteractAnimation(ELootState.Loop);
                        break;
                    case ELootState.End:
                        playerComponent.SetLootInteractAnimation(ELootState.End);
                        break;
                    default:
                        break;

                }
            }
        }

        private ELootState lootState = ELootState.None;
        public int stateCount = 0;
        public string Message => $" {this.name} Loot Start";

        public float sqrInteractRange = 5f;

        public float startFinishTime;
        public float keepFinishTime;
        public float loopFinishTime;
        public float endFinishTime;

        CharacterBase playerComponent;
        public void Interact(GameObject go)
        {
            if (lootState != ELootState.None)
                return;

            if (go.TryGetComponent(out CharacterBase playerComponent) == false)
                return;

            Vector3 itemPos = this.gameObject.transform.position;
            Vector3 playerPos = playerComponent.transform.position;

            float sqrDistMagnitude = Vector3.SqrMagnitude(itemPos - playerPos);
            if (sqrDistMagnitude > sqrInteractRange)
                return;


            this.playerComponent = playerComponent;

            lootState = ELootState.Start;
            playerComponent.SetLootInteractAnimation(ELootState.Start);

            Debug.Log($"<b><color=red> {Message}!</color></b>");
        }

        public void Update()
        {
            // ¸Å¹ø °Ë»çÇÏ´Â°Å Èì³Ä.. ÂòÂòÇÏ±¸¸¸
            if (playerComponent == null)
                return;

            switch (lootState)
            {
                case ELootState.Start:
                    LootStart();
                    break;
                case ELootState.Keep:
                    LootKeep();
                    break; 
                case ELootState.Loop:
                    LootLoop();
                    break;
                case ELootState.End:
                    LootEnd();
                    break;
                default:
                    break;
            }

        }

        void LootStart()
        {
            AnimatorStateInfo stateInfo = playerComponent.animator.GetCurrentAnimatorStateInfo(0);
            float animPlayTime = stateInfo.normalizedTime;

            //if (animPlayTime > startFinishTime)

        }

        void LootKeep()
        {

        }

        void LootLoop()
        {

        }

        void LootEnd()
        {

        }

        // state °¹¼ö Ã¼Å©¿ë
        private void OnValidate()
        {
            if (stateCount ==  0)
            {
                Debug.LogError("Loot Animation State Count Check Needed!", this);
            }
        }
    }
}
