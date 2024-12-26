using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TST.LootAnimation;

namespace TST
{
    public class LootBackPack : LootBase
    {
        public override void SetInitialize()
        {
            message = "LootBackPack";
            //interactRange = 5.0f;
            lootTime = 5.0f;
        }

        public override void SetInteractLootType(CharacterBase playerComponent)
        {
            if (playerComponent == null)
                Debug.Log($"playerComponent is Null");

            playerComponent.SetLootType((float)ELootType.BackPack);
        }
    }
}
