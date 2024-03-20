using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class Sensor : CoreComponent
    {
        [Header("SIZE OF SENSOR")]
        [SerializeField] private float cellingRadius;
        [SerializeField] private float groundDistance;
        [SerializeField] private float wallDistance;
        [SerializeField] private float ledgePrecision;

        [field: Header("POSITION OF SENSOR")]
        [field: SerializeField] public Transform CeilingSensor { get; private set; }
        [field: SerializeField] public Transform GroundSensor { get; private set; }
        [field: SerializeField] public Transform WallSensor { get; private set; }
        [field: SerializeField] public Transform HorizontalSensor { get; private set; }

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

        private RaycastHit2D LedgeHitUp => Physics2D.Raycast(
            HorizontalSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            wallDistance,
            PlatformsLayer);

        private RaycastHit2D LedgeHitDown => Physics2D.Raycast(
            new Vector2(HorizontalSensor.position.x, HorizontalSensor.position.y - ledgePrecision),
            Vector2.right * core.Movement.FacingDirection,
            wallDistance,
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
                HorizontalSensor.position, Vector2.down,
                ledgePrecision, PlatformsLayer) && !LedgeHitUp && LedgeHitDown;

            if (!isLedge) return false;
           
            RaycastHit2D cornerRay = Physics2D.Raycast(
                new Vector2(LedgeHitDown.point.x, HorizontalSensor.position.y), Vector2.down,
                ledgePrecision, PlatformsLayer);
            
            if (!cornerRay) return false;

            ledgeCorner = cornerRay.point;

            return cornerRay;
        }

        public float GetGroundSlopeAngle() => Vector2.Angle(GroundHit.normal, Vector2.up);

        public Vector2 GetGroundPerperdicular() => Vector2.Perpendicular(core.Sensor.GroundHit.normal).normalized;

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
           
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(CeilingSensor.position, cellingRadius); //celling ray 
            Gizmos.DrawRay(GroundSensor.position, new Vector2(0, -groundDistance)); //ground ray
            Gizmos.DrawRay(WallSensor.position, new Vector2(wallDistance * core.Movement.FacingDirection, 0)); //wall ray
            Gizmos.DrawRay(HorizontalSensor.position, new Vector2(wallDistance * core.Movement.FacingDirection, 0)); //ledge ray up
            Gizmos.DrawRay(new Vector3(HorizontalSensor.position.x, HorizontalSensor.position.y - ledgePrecision), 
                new Vector2(wallDistance * core.Movement.FacingDirection, 0)); //ledge ray down           
        }
    }

}
