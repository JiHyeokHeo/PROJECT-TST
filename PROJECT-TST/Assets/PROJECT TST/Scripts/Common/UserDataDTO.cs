using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    [System.Serializable]
    public class UserDataDTO<T>
    {
        // 이름 항상 Values로 고정 // CsvToJson csv 컨버팅 관련쪽에서 이름 Values로 고정해놨음
        public List<T> Values = new List<T>();
    }

    [System.Serializable]
    public class SkillDataDTO{ }

    [System.Serializable]
    public class IngamePlayerSkillDTO
    {
        public int SkillDataId;

    }

    //[System.Serializable]
    //public class IngamePlayerDataDTO : UserDataDTO
    //{
    //    public int DataId;
    //    public string Name;
    //    public float Hp;
    //    public float AttackPower;
    //    public float Speed;
    //    public List<int> SkillData;
    //    public Vector3 PlayerPosition; 
    //    public Quaternion PlayerRotation;
    //    public List<string> Equipments;
    //}

    [System.Serializable]
    public class IngamePlayerDataDTO : UserDataDTO<IngamePlayerDataDTO>
    {
        public string Name;
        public Vector3 Position;
        public List<int> Skills;
    }
}
