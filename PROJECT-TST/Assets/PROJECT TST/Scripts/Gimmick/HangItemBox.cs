using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class HangItemBox : MonoBehaviour, IDamage
    {
        //private bool isHanging = true;

        public void ApplyDamage(float damage)
        {
            //if (isHanging)
            //{
                Rigidbody boxRigid = GetComponentInChildren<Rigidbody>();
                boxRigid.isKinematic = false;
                //isHanging = false;
            //}
        }
    }
}
