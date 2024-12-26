using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class LootTableTop : LootBase
    {
        public override void SetInitialize()
        {
            message = "LootTableTop";
            //interactRange = 5.0f;
            //lootTime = 5.0f;
        }

        public override void SetInteractLootType(CharacterBase playerComponent)
        {
            if (playerComponent == null)
                Debug.Log($"playerComponent is Null");

            playerComponent.SetLootType((float)ELootType.TableTop);
        }
    }
}
