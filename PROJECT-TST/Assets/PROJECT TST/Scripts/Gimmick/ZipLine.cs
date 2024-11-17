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
        [SerializeField] private LineRenderer cable;

        public Transform zipTransfom;

        public string Message => "집라인 연결";

        private bool zipping = false;

        // 매달리고 있는 구체
        private GameObject localZip;

        void Awake()
        {
            cable.SetPosition(0, zipTransfom.position);
            cable.SetPosition(1, targetZip.zipTransfom.position);
        }

        public void Interact(GameObject go)
        {
            Vector3 playerPosition = go.transform.position;
            float distSqr = Vector3.SqrMagnitude(playerPosition - this.gameObject.transform.position);

            if (distSqr < 20.0f)
            {
                StartZipline(go);
            }
        }

        private void Update()
        {
            if (!zipping || localZip == null)
                return;

            localZip.GetComponent<Rigidbody>().AddForce((targetZip.zipTransfom.position - zipTransfom.position).normalized * zipSpeed * Time.deltaTime, ForceMode.Acceleration);

            if (Vector3.Distance(localZip.transform.position, targetZip.zipTransfom.position) < arrivalThreshold ) 
            {
                RestZipLine();
            }
        }

        private void StartZipline(GameObject player)
        {
            if (zipping)
                return;

            localZip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            localZip.transform.position = zipTransfom.position;
            localZip.transform.localScale = new Vector3(zipScale, zipScale, zipScale);
            localZip.AddComponent<Rigidbody>().useGravity = false;
            localZip.GetComponent<Collider>().isTrigger = true;

            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<CharacterController>().enabled= false;
            player.transform.parent = localZip.transform;
            zipping = true;
        }

        private void RestZipLine()
        {
            if (!zipping)
                return;

            GameObject player = localZip.transform.GetChild(0).gameObject;
            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<CharacterController>().enabled = true;
            player.transform.parent = null;
            Destroy(localZip);
            localZip = null ;
            zipping = false;
            Debug.Log("Zipline reset");
        }
    }
}
