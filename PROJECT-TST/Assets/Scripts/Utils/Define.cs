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

