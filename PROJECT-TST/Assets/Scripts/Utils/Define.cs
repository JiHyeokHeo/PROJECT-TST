using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defines
{
    public static class Define
    {
        public enum ECharactorState
        {
            Idle,
            Move,
            Attack,
            Skill,
            Damaged, // ������ �������� ���� �ƴ� ����
            Die,
        }

        public enum ESceneType
        {
            None = 0,


            End = 16,
        }

        public enum EInputType
        {
            KeyBoard,
            Mouse,
            None,
        }

        public enum EInputActionType
        {
            Performed,
            Canceled,
            None,
        }
    }

}

// HARDCODING ���� ScriptableObject�� ���� ����    [SerializeField]

public static class HARDCODING
{
    public static float MOVESPEEDTEMP = 5.0f;
    public static float ANGLELIMITTEMP = 60.0f;
    public static float ROTATIONSPEEDTEMP = 250.0f;
}



