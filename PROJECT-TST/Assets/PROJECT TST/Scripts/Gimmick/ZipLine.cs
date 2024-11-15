using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class ZipLine : MonoBehaviour, IInteractable
    {
        [SerializeField] private ZipLine targetZip;
        [SerializeField] private float zipSpeed = 5f;
        [SerializeField] private float zipScale = 0.2f;

        [SerializeField] private float arrivalThreshold = 0.4f;

        public Transform zipTransfom;

        public string Message => "집라인 연결";

        private bool zipping = false;
        private GameObject localZip;

        public void Interact()
        {
            Debug.Log(Message);
        }

        public void StartZipline(GameObject player)
        {
            localZip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            localZip.transform.position = zipTransfom.position;
            localZip.transform.localScale = new Vector3(zipScale, zipScale, zipScale);
            localZip.AddComponent<Rigidbody>().useGravity = false;
            localZip.GetComponent<Collider>().isTrigger = true;

            player.GetComponent<Rigidbody>().useGravity = false;
        }

        private void RestZipLine()
        {

        }
    }
}
