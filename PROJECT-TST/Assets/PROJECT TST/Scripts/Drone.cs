using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TST
{
    public class Drone : MonoBehaviour
    {
        public GameObject owner;

        public Transform droneTransform;

        public GameObject leftWing;
        public GameObject rightWing;

        public GameObject gun;
        public Transform missilePocket;

        public GameObject missilePrefab;

        public bool IsShowing
        {
            get => isShowing;
            set
            {
                if (isChangingState)
                    return;

                isShowing = value;
                if (isShowing)
                    gameObject.SetActive(true);

                isPlayed = false;
                changeMovement = isShowing ? ShowMovement : HideMovement;
                isChangingState = true;
            }
        }

        private bool isChangingState = false;
        private bool isShowing = false;

        private Action changeMovement;

        private void Start()
        {
            droneTransform = GetComponent<Transform>();
            transform.position = endPoint.position;
        }

        private void Update()
        {
            if (isChangingState)
                changeMovement?.Invoke();

            if (owner != null && !isChangingState)
                FollowOwner();
        }

        bool isPlayed = false;
        private void ShowMovement()
        {
            if (isChangingState == false)
                return;

            if (isPlayed == false)
            {
                StartParabolicMovement(endPoint.position, startPoint.position);
                isPlayed = true;
            }
        }

        private void HideMovement()
        {
            if (isChangingState == false)
                return;

            if (isPlayed == false)
            {
                StartParabolicMovement(startPoint.position, endPoint.position);
                isPlayed = true;
            }
        }

        private float CheckSqrDistance(Vector3 finalDestination)
        {
            Vector3 dir = transform.position - finalDestination;

            return Vector3.SqrMagnitude(dir);
        }

        public Transform startPoint;
        public Transform endPoint;
        public float height = 1f;    // 포물선 높이
        public float duration = 1f;  // 이동 시간

        public void StartParabolicMovement(Vector3 startPosition, Vector3 endPosition)
        {
            Vector3 peak = (startPosition + endPosition) / 2 + Vector3.up * height; // 정점 위치

            transform.DOKill();
            // 포물선 애니메이션
            transform.DOPath(new Vector3[] { startPosition, peak, endPosition }, duration, PathType.CatmullRom)
                     .SetEase(Ease.OutQuad)
                     .OnComplete(() =>
                     {
                         isChangingState = false;
                     });
                    
        }

        public void SetOwner(GameObject owner)
        {
            this.owner = owner;
        }


        public float changeInterval = 2.0f; // 랜덤 회전 목표 변경 간격

        private Quaternion targetRotation; // 목표 회전 값
        private float timeElapsed = 0.0f; // 마지막 랜덤 변경 이후 경과 시간

        private void FollowOwner()
        {
            if (owner == null)
                return;

            if (isShowing == false)
                return;

            transform.position = startPoint.position;

            // 주기적으로 랜덤 목표 회전 값 업데이트
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= changeInterval)
            {
                timeElapsed = 0.0f;

                // 랜덤 목표 회전 값 생성
                Vector3 randomRotation = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-30.0f, 30.0f), 0.0f);

                targetRotation = Quaternion.LookRotation(owner.transform.forward) * Quaternion.Euler(randomRotation);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / 0.5f);
        }
    }
}
