using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
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
    }

}

