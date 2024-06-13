using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    public class CeilingDetector : SensorDetectComponent
    {
        [SerializeField] private float circleRadius;
        [SerializeField] private float positionOffsetY;
        [SerializeField] private LayerMask targetLayer;

        protected override string SensorName => nameof(CeilingDetector);

        protected override Vector2 InitSensorPosition =>
            new(entityCollider.bounds.center.x, entityCollider.bounds.center.y + positionOffsetY);


        public Collider2D CeilingCollider => Physics2D.OverlapCircle(
            sensor.position,
            circleRadius,
            targetLayer);

        public bool IsCeilingDetect() => CeilingCollider;


        protected override void DrawRay()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(sensor.position, circleRadius);
        }
    }
}
