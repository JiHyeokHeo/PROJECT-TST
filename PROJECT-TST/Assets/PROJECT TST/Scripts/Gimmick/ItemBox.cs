using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class ItemBox : MonoBehaviour, IInteractable
    {
        // UI 같은 부분 추가 할 때 필요한 메시지
        public string Message => "Pick up";

        public void Interact(GameObject go)
        {
            Debug.Log("<color=red <b>Item Box</b> Interacted ! </color>");

            Destroy(gameObject);
        }
    }
}
