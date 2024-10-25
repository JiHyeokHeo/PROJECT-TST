using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; }


        [field: SerializeField] public bool IsCameraZoom { get; set; } = false;
        [field: SerializeField] public bool IsCameraSideOnRight { get; set; } = true;


        public Cinemachine.CinemachineVirtualCamera tpsCamera;
        public Vector2 cameraDistance = new Vector2(2f, 1.0f);

        private Cinemachine3rdPersonFollow tpsCameraFollow;
        private float blendCameraSide;
        private float blendCameraDistance;

        private void Awake()
        {
            Instance = this;
            tpsCameraFollow = tpsCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }

        private void Update()
        {
            blendCameraDistance = Mathf.Lerp(blendCameraDistance, IsCameraZoom ? cameraDistance.y : cameraDistance.x, Time.deltaTime * 10f);
            blendCameraSide = Mathf.Lerp(blendCameraSide, IsCameraSideOnRight ? 1 : 0, Time.deltaTime * 10f);

            tpsCameraFollow.CameraDistance = blendCameraDistance;
            tpsCameraFollow.CameraSide = blendCameraSide;
        }
    }
}
