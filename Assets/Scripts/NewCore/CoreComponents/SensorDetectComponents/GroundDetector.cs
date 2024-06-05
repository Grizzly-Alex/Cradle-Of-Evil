using NewCore.CoreComponents.SensorDetectComponents;
using NewCoreSystem;
using System;
using UnityEngine;

namespace NewCore.CoreComponents.PhysicsComponents
{
    [Serializable]
    public class GroundDetector : SensorDetectComponent
    {
        private readonly float inactiveGroundSensorDistance;
        private readonly Vector2 sensorPosition;

        [SerializeField] private float hitDistance;
        [SerializeField] public LayerMask targetLayer;
        [SerializeField] public string platformTag;
        [SerializeField] public string oneWayPlatformTag;


        public GroundDetector(Core core) : base(core)
        {
            sensorPosition = entityCollider.bounds.center;
            inactiveGroundSensorDistance = sensorPosition.y - entityCollider.bounds.min.y;
        }

        public RaycastHit2D GroundHit => Physics2D.Raycast(
            sensorPosition,
            Vector2.down,
            Mathf.NegativeInfinity,
            targetLayer);

        public bool IsGroundDetect()
            => hitDistance >= GroundHit.distance;

        public bool IsPlatformDetect()
            => GroundHit.collider.CompareTag(platformTag)
            && hitDistance >= GroundHit.distance;

        public bool IsOneWayPlatformDetect()
            => GroundHit.collider.CompareTag(oneWayPlatformTag)
            && hitDistance >= GroundHit.distance
            && inactiveGroundSensorDistance <= GroundHit.distance;

        public float GetGroundSlopeAngle()
            => Vector2.Angle(GroundHit.normal, Vector2.up);

        public bool IsGroundSlope()
            => GetGroundSlopeAngle() != default;

        public Vector2 GetGroundPerperdicular()
            => Vector2.Perpendicular(GroundHit.normal).normalized;


        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(sensorPosition, new Vector2(0, -hitDistance));
        }
    }
}
