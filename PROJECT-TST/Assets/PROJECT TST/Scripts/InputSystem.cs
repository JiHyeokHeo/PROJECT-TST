using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class InputSystem : MonoBehaviour
    {
        private void Start()
        {
            SetCursorVisible(false);
        }

        private static void SetCursorVisible(bool isVisible)
        {
            if (isVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                SetCursorVisible(true);
            }
            else
            {
                SetCursorVisible(false);
            }
        }
    }
}
