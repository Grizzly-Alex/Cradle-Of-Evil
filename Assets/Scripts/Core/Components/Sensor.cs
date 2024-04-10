using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class Sensor : CoreComponent
    {
        [Header("SIZE OF SENSOR")]
        [SerializeField] private float cellingRadius;
        [SerializeField] private float groundDistance;
        [SerializeField] private float wallDistance;

        [field: Header("POSITION OF SENSOR")]
        [field: SerializeField] public Transform CeilingSensor { get; private set; }
        [field: SerializeField] public Transform GroundSensor { get; private set; }
        [field: SerializeField] public Transform WallSensor { get; private set; }
        [field: SerializeField] public Transform LedgeSensor { get; private set; }

        [field: Header("LAYER MASK")]
        [field: SerializeField] public LayerMask TerrainLayer { get; private set; }

        [field: Header("TAG MASK")]
        [field: SerializeField] public string Platform { get; private set; }
        [field: SerializeField] public string GrabWall { get; private set; }
        [field: SerializeField] public string Ledge { get; private set; }

        #region Sensors
        public RaycastHit2D GroundHit => Physics2D.Raycast(
            GroundSensor.position,
            Vector2.down,
            groundDistance,
            TerrainLayer);

        public RaycastHit2D WallHit => Physics2D.Raycast(
            WallSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            wallDistance,
            TerrainLayer);

        private RaycastHit2D LedgeHitUp => Physics2D.Raycast(
            LedgeSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            wallDistance,
            TerrainLayer);

        public Collider2D Circle => Physics2D.OverlapCircle(CeilingSensor.position, cellingRadius, TerrainLayer);
        #endregion

        public bool IsGroundDetect() => GroundHit;

        public bool IsCellingDetect() => Circle;

        public bool IsGrabWallDetect() => 
            WallHit.collider != null
            && WallHit.collider.CompareTag(GrabWall);

        public bool IsLedgeDetect()
        {
            bool behindIsEmpty = !Physics2D.Raycast(
                LedgeSensor.position,
                Vector2.left * core.Movement.FacingDirection,
                wallDistance,
                TerrainLayer);

            return LedgeHitUp.collider != null
                && LedgeHitUp.collider.CompareTag(Ledge)
                && behindIsEmpty;
        }

        public bool GetDetectedGrabWallPosition(out Vector2 wallPosition)
        {
            wallPosition = Vector2.zero;
            bool isDetected = IsGrabWallDetect();

            if (isDetected)
            {
                Bounds colliderBounds = WallHit.collider.bounds;
                wallPosition.Set(colliderBounds.center.x, WallHit.point.y);
            }

            return isDetected;

        }

        public bool GetDetectedLedgeCorner(out Vector2 ledgeCorner)
        {
            ledgeCorner = Vector2.zero;
            bool isDetected = IsLedgeDetect();

            if (isDetected)
            {
                Bounds colliderBounds = LedgeHitUp.collider.bounds;
                ledgeCorner.Set(colliderBounds.center.x, colliderBounds.max.y);
            }

            return isDetected;
        }

        public float GetGroundSlopeAngle() 
            => Vector2.Angle(GroundHit.normal, Vector2.up);

        public Vector2 GetGroundPerperdicular() 
            => Vector2.Perpendicular(core.Sensor.GroundHit.normal).normalized;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
           
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(CeilingSensor.position, cellingRadius); //celling ray 
            Gizmos.DrawRay(GroundSensor.position, new Vector2(0, -groundDistance)); //ground ray
            Gizmos.DrawRay(WallSensor.position, new Vector2(wallDistance * core.Movement.FacingDirection, 0)); //wall ray
            Gizmos.DrawRay(LedgeSensor.position, new Vector2(wallDistance * core.Movement.FacingDirection, 0)); //ledge ray up
            Gizmos.DrawRay(LedgeSensor.position, new Vector2(wallDistance * -core.Movement.FacingDirection, 0)); //ledge ray behind      
        }
    }
}
