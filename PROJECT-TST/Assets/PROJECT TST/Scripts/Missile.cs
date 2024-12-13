using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TST
{
    public class Missile : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Target _target;
        [SerializeField] private GameObject _explosionPrefab;

        [Header("MOVEMENT")]
        [SerializeField] private float _speed = 15;
        [SerializeField] private float _rotateSpeed = 95;

        // 예측
        [Header("PREDICTION")]
        [SerializeField] private float _maxDistancePredict = 100;
        [SerializeField] private float _minDistancePredict = 5;
        [SerializeField] private float _maxTimePrediction = 5;
        private Vector3 _standardPrediction, _deviatedPrediction;

        // 편차
        [Header("DEVIATION")]
        [SerializeField] private float _deviationAmount = 50;
        [SerializeField] private float _deviationSpeed = 2;

        // 궁금한 점 : 미사일 타겟을 잡을 때 마우스 좌클릭을 실행하는 동시에 캐릭터 주변(CharacterBase or Controller) Phyiscs.OverlapSphere 같은 것을 활용해
        // Target이라는 스크립트를 갖고 있는 친구들을 찾아 거리 or 랜덤 으로 타겟을 설정시켜 쏘는 것이 좋을지 
        
        private void FixedUpdate()
        {
            if (gameObject.activeSelf == false)
                return;

            // 진행만 시키도록 하고
            _rb.velocity = transform.forward * _speed;

            if (_target == null)
                return;
            // 정규화 a~b 0~1 value
            // 가까울 수록 편차가 줄고, 멀수록 편차가 커짐
            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));

            // 1차 예측
            PredictMovement(leadTimePercentage);

            // 편차 추가
            AddDeviation(leadTimePercentage);

            // 로테이션
            RotateRocket();
        }

        public void SetTarget(Target target)
        {
            _target = target;
        }

        private void PredictMovement(float leadTimePercentage)
        {
            var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);
            
            // 예측 위치 계산의 수학적 원리 // 주로 유도 미사일, AI추적, 스포츠게임(플레이어와 공 이동 예상 인터셉트)
            // 예측 위치 = 현재위치 + (속도 x 시간)
            _standardPrediction = _target.Rb.position + _target.Rb.velocity * predictionTime;
        }

        // 예측된 위치에 편차를 추가하는 기능
        private void AddDeviation(float leadTimePercentage)
        {
            // 목표의 예측 위치를 x축 방향으로 흔들리게 하는 역할을 합니다. // Cos보다 PerlinNoise를 활용하면 더욱 예측하기 어려운 움직임이 가능하다.
            var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

            // world 좌표의 방향 * 스칼라값 * 예측 시간에 따라 편차 크기 변경 -> 예측 시간이 짧을 수록 편차가 적다
            var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

            _deviatedPrediction = _standardPrediction + predictionOffset;
        }

        private void RotateRocket()
        {
            // 도착 방향벡터
            var heading = _deviatedPrediction - transform.position;
             
            var rotation = Quaternion.LookRotation(heading);
        
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_explosionPrefab) Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            if (collision.transform.TryGetComponent<Target>(out var ex))
                ex.ApplyDamage(1);

            //Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _standardPrediction);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_standardPrediction, _deviatedPrediction);
        }
    }
}
