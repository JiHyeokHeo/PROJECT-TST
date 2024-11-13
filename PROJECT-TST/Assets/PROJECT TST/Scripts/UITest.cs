using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TST
{
    public class UITest : MonoBehaviour, IPointerClickHandler
    {
        public UITest crossHair;
        public event Action<PointerEventData> OnClickHandler;

        void OnEnable()
        {
            OnClickHandler += (evt) =>
            {
                if (OptionManager.Instance.usingCrossHair != crossHair)
                {
                    OptionManager.Instance.ChangeCrossHair(crossHair);
                }
            };
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickHandler?.Invoke(eventData);
        }
    }
}
