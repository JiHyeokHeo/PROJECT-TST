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
            Damaged, // 경직을 넣을꺼면 쓰고 아님 말자
            Die,
        }

        public enum ESceneType
        {
            None = 0,


            End = 16,
        }
    }

}

