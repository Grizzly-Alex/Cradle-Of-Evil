using NewCore.CoreComponents.SensorDetectComponents;
using NewCoreSystem;
using System;
using UnityEngine;

namespace Assets.Scripts.NewCore.CoreComponents.SensorDetectComponents
{
    [Serializable]
    public class CeilingDetector : SensorDetectComponent
    {
        private readonly Vector2 sensorPosition;

        [SerializeField] private float circleRadius;
        [SerializeField] private float positionOffsetY;
        [SerializeField] public LayerMask targetLayer;


        public CeilingDetector(Core core) : base(core)
        {
            sensorPosition = new Vector2(entityCollider.bounds.max.y - positionOffsetY, entityCollider.bounds.center.x);
        }

        public Collider2D CeilingCollider => Physics2D.OverlapCircle(
            sensorPosition,
            circleRadius,
            targetLayer);

        public bool IsCeilingDetect() => CeilingCollider;


        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(sensorPosition, circleRadius);
        }
    }
}
