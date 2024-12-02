using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TST
{
    public class UITest : MonoBehaviour, IPointerClickHandler
    {
        
        public event Action<PointerEventData> OnClickHandler;

        void OnEnable()
        {
            OnClickHandler += (evt) =>
            {
                if (OptionManager.Instance.usingCrossHair.name != gameObject.name)
                {
                    CrossHairType type = OptionManager.StringToEnum<CrossHairType>(gameObject.name);
                    OptionManager.Instance.ChangeCrossHair(type);
                }
            };
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickHandler?.Invoke(eventData);
        }
    }
}
