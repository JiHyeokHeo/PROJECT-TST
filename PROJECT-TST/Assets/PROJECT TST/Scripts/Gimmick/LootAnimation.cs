using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TST
{
    public class LootAnimation : MonoBehaviour, IInteractable
    {
        private const int ERRORNUM = -999999;

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
                    case ELootState.None:
                        playerComponent.SetLootInteractAnimation(ELootState.None);
                        break;
                    default:
                        break;

                }
            }
        }

        private ELootState lootState = ELootState.None;
        public int lootType = ERRORNUM;
        public string Message => $" {this.name} Loot Start";

        public float sqrInteractRange = 5f;

        public float keepFinishTime;
        public float loopFinishTime;

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

            playerComponent.SetLootType(lootType);
            // 버그 생성지점
            LootState = ELootState.Start;
            playerComponent.SetInteractAnimation(EInteractionType.Looting); 
            Debug.Log($"<b><color=red> {Message}!</color></b>");
        }

        public void Update()
        {
            if (lootState == ELootState.None)
                return;

            if (playerComponent == null)
                return;

            // 최대한 여기서 애니메이터 파라미터를 받아와서 작업하는게 좋아보이는데에...흐음
            float moveMagnitude = playerComponent.animator.GetFloat("Magnitude");
            if (moveMagnitude > 0.1f)
                LootState = ELootState.None;
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

        AnimatorStateInfo stateInfo;
        void LootStart()
        {
            stateInfo = playerComponent.animator.GetCurrentAnimatorStateInfo(0);
            float currentTime = stateInfo.normalizedTime * stateInfo.length;

            // stateInfo If문 내에서 한번만 가져오도록 할 수 있을 듯함 매 프레임 찾을 필요X
            if (currentTime >= stateInfo.length)
                LootState = ELootState.Keep;
        }

        void LootKeep()
        {
            stateInfo = playerComponent.animator.GetCurrentAnimatorStateInfo(0);
            float currentTime = stateInfo.normalizedTime * stateInfo.length;
            
            if (currentTime > keepFinishTime)
                LootState = ELootState.Loop;
        }

        void LootLoop()
        {
            stateInfo = playerComponent.animator.GetCurrentAnimatorStateInfo(0);
            float currentTime = stateInfo.normalizedTime * stateInfo.length;

            if (currentTime > loopFinishTime)
                LootState = ELootState.End;
        }

        void LootEnd()
        {
            stateInfo = playerComponent.animator.GetCurrentAnimatorStateInfo(0);
            float currentTime = stateInfo.normalizedTime * stateInfo.length;
            
            if (currentTime >= stateInfo.length)
                LootState = ELootState.None;
        }

        // state 갯수 체크용
        private void OnValidate()
        {
            if (lootType == ERRORNUM)
            {
                Debug.LogError("Loot Animation LootType Check Needed!", this);
            }
        }
    }
}
