using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class LootDresser : LootBase
    {
        public override void SetInitialize()
        {
            message = "LootDresser";
            //interactRange = 5.0f;
            //lootTime = 5.0f;
        }

        public override void SetInteractLootType(CharacterBase playerComponent)
        {
            if (playerComponent == null)
                Debug.Log($"playerComponent is Null");

            playerComponent.SetLootType((float)ELootType.Dresser);
        }
    }
}
