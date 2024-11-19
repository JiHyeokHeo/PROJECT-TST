using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TST
{
    [Serializable]
    public struct ProjectileProperties
    {
        public Vector3 direction;
        public Vector3 initialPosition;
        public float initialSpeed;
        public float mass;
        public float drag;
    }

    public class BombProjectile : ProjectileBase
    {
        [SerializeField]
        TrajectoryPredictor trajectoryPredictor;

        [SerializeField]
        ProjectileProperties projectileProperties;

        protected override void Init()
        {
            rigid = GetComponent<Rigidbody>();
            if (rigid == null )
                rigid = this.AddComponent<Rigidbody>();

            startPosition = transform;

            rigid.AddForce(transform.forward * moveForce, ForceMode.Impulse);

        }

        void Update()
        {
            if (trajectoryPredictor != null)
                trajectoryPredictor.PredictTrajectory(ProjectileData());
        }

        ProjectileProperties ProjectileData()
        {
            ProjectileProperties properties = new ProjectileProperties();
            properties.direction = startPosition.forward;
            properties.initialPosition = startPosition.position;
            properties.initialSpeed = moveForce;
            properties.mass = rigid.mass;
            properties.drag = rigid.drag;

            projectileProperties = properties;

            return properties;
        }
    }
}
