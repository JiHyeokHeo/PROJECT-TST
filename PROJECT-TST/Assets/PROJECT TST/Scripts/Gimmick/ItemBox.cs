using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class ItemBox : MonoBehaviour, IInteractable
    {
        public string Message => "Pick up";
        public void Interact()
        {
            Debug.Log("<color=red> <b>Item Box</b> Interacted ! </color>");
            Destroy(gameObject);
        }
    }
}
