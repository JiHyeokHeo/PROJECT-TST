using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    [System.Serializable]
    public class UserDataDTO { }

    [System.Serializable]
    public class IngamePlayerDataDTO : UserDataDTO
    {
        public Vector3 playerPosition;
        public Quaternion playerRotation;
    }
}
