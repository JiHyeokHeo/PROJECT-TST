using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    [System.Serializable]
    public class RootDataDTO
    {
        public int ID;
    }

    [System.Serializable]
    public class UserDataDTO : RootDataDTO
    {
       
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
    public class IngamePlayerDataDTO : UserDataDTO
    {
        public string Name;
        public Vector3 Position;
        public List<int> Skills;
    }
}
    