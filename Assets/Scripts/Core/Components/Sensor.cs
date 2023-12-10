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
        [field: SerializeField] public Transform HorizontalLedgeSensor { get; private set; }
        [field: SerializeField] public Transform VerticalLedgeSensor { get; private set; }

        [field: Header("LAYER MASK")]
        [field: SerializeField] public LayerMask PlatformsLayer { get; private set; }

        [field: Header("TAG MASK")]
        [field: SerializeField] public string Ground { get; private set; }
        [field: SerializeField] public string GrabWall { get; private set; }

        #region Sensors
        public RaycastHit2D GroundHit => Physics2D.Raycast(
            GroundSensor.position,
            Vector2.down,
            groundDistance,
            PlatformsLayer);

        public RaycastHit2D WallHit => Physics2D.Raycast(
            WallSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            wallDistance,
            PlatformsLayer);

        public RaycastHit2D LedgeHorizontalHit => Physics2D.Raycast(
            HorizontalLedgeSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            wallDistance,
            PlatformsLayer);

        public RaycastHit2D VerticalLedgeHit => Physics2D.Raycast(
            new Vector2(VerticalLedgeSensor.position.x * core.Movement.FacingDirection, VerticalLedgeSensor.position.y),
            Vector2.down,
            groundDistance,
            PlatformsLayer);

        public Collider2D Circle => Physics2D.OverlapCircle(CeilingSensor.position, cellingRadius, PlatformsLayer);
        #endregion

        public bool IsWallDetect() => WallHit;

        public bool IsGroundDetect() => GroundHit;

        public bool IsCellingDetect() => Circle;
        
        public bool IsGrabWallDetect() => WallHit.collider != null && WallHit.collider.CompareTag(GrabWall);

        public bool IsHorizonalLedgCornerDetect(out Vector2 ledgeCorner)
        {
            ledgeCorner = Vector2.zero;

            bool isLedge = !Physics2D.Raycast(
                HorizontalLedgeSensor.position, Vector2.down,
                HorizontalLedgeSensor.position.y - WallSensor.position.y, PlatformsLayer) && !LedgeHorizontalHit && WallHit;

            if (!isLedge) return false;
           
            RaycastHit2D cornerRay = Physics2D.Raycast(
                new Vector2(WallHit.point.x, HorizontalLedgeSensor.position.y), Vector2.down,
                HorizontalLedgeSensor.position.y - WallSensor.position.y, PlatformsLayer);
            
            if (!cornerRay) return false;

            ledgeCorner = cornerRay.point;

            return cornerRay;
        }

        public float GetGroundSlopeAngle() => Vector2.Angle(GroundHit.normal, Vector2.up);

        public Vector2 GetGroundPerperdicular() => Vector2.Perpendicular(core.Sensor.GroundHit.normal).normalized;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(CeilingSensor.position, cellingRadius); //celling ray 
            Gizmos.DrawRay(GroundSensor.position, new Vector2(0, - groundDistance)); //ground ray
            Gizmos.DrawRay(WallSensor.position, new Vector2(- wallDistance, 0)); //wall ray
            Gizmos.DrawRay(HorizontalLedgeSensor.position, new Vector2(- wallDistance, 0)); //ledge horizontal ray    
        }
    }

}
