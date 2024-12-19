using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "PROJECT TST/Item/Item Data")]
    public class ItemData : ScriptableObject
    {
        [field: SerializeField] public string ItemID { get; private set; }

        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public GameObject ItemVisualPrefab { get; private set; }
    }
}
