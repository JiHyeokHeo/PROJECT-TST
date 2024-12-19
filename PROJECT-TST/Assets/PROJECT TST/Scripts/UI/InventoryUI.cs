using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class InventoryUI : UIBase
    {
        public void OnClickCloseButton()
        {
            UIManager.Hide<InventoryUI>(UIList.InventoryUI);
        }
    }
}
