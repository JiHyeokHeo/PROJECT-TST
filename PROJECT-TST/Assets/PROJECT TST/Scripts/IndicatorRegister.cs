using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class IndicatorRegister : MonoBehaviour
    {
        private void Start()
        {
            IndicatorUI.Instance.RegistIndicator(transform);
        }
    }
}
