using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class Sensor : CoreComponent
    {
        [SerializeField] private Grid grid;
        private CapsuleCollider2D entityCollider;

        private float inactiveGroundSensorDistance;

        [Header("SIZE OF SENSOR")]
        [SerializeField] private float cellingRadius;
        [SerializeField] private float groundDistance;
        [SerializeField] private float wallDistance;
        [SerializeField] private float ledgeDistance;
        [SerializeField] private float spanOfLedge;
        [SerializeField] private float spanOfGrabWall;

        [field: Header("POSITION OF SENSORS")]
        [field: SerializeField] public Transform CeillingSensor { get; private set; }
        [field: SerializeField] public Transform GroundSensor { get; private set; }
        [field: SerializeField] public Transform WallSensor { get; private set; }
        [field: SerializeField] public Transform LedgeSensor { get; private set; }

        [field: Header("LAYER MASK")]
        [field: SerializeField] public LayerMask TargetLayerForGroundSensor { get; private set; }
        [field: SerializeField] public LayerMask TargetLayerForCeillingSensor { get; private set; }
        [field: SerializeField] public LayerMask TargetLayerForWallSensor { get; private set; }
        [field: SerializeField] public LayerMask TargetLayerForLedgeSensor { get; private set; }

        [field: Header("TAG MASK")]
        [field: SerializeField] public string Platform { get; private set; }
        [field: SerializeField] public string OneWayPlatform { get; private set; }
        [field: SerializeField] public string GrabWall { get; private set; }

        #region Sensors
        public RaycastHit2D GroundHit => Physics2D.Raycast(
            GroundSensor.position,
            Vector2.down,
            Mathf.NegativeInfinity,
            TargetLayerForGroundSensor);

        public RaycastHit2D WallHit => Physics2D.Raycast(
            WallSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            wallDistance,
            TargetLayerForWallSensor);

        private RaycastHit2D LedgeHit => Physics2D.Raycast(
            LedgeSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            ledgeDistance,
            TargetLayerForLedgeSensor);

        public Collider2D Circle => Physics2D.OverlapCircle(CeillingSensor.position, cellingRadius, TargetLayerForCeillingSensor);
        #endregion

        protected override void Start()
        {
            base.Start();
            entityCollider = GetComponentInParent<CapsuleCollider2D>();
            inactiveGroundSensorDistance = GroundSensor.position.y - entityCollider.bounds.min.y;
        }

        public bool IsGroundDetect()
            => groundDistance >= GroundHit.distance;

        public bool IsPlatformDetect() 
            => GroundHit.collider.CompareTag(Platform)
            && groundDistance >= GroundHit.distance;

        public bool IsOneWayPlatformDetect()
            => GroundHit.collider.CompareTag(OneWayPlatform)
            && groundDistance >= GroundHit.distance
            && inactiveGroundSensorDistance <= GroundHit.distance;

        public bool IsCellingDetect() => Circle;

        public bool IsGrabWallDetect()
        {
            RaycastHit2D spanRayHit = Physics2D.Raycast(
                new Vector2(WallSensor.position.x, WallSensor.position.y - spanOfGrabWall),
                Vector2.right * core.Movement.FacingDirection,
                wallDistance,
                TargetLayerForWallSensor);

            if (WallHit.collider is null || spanRayHit.collider is null) return false;
                                   
            return WallHit.collider.CompareTag(GrabWall) && spanRayHit.collider.CompareTag(GrabWall);           
        }

        public bool IsLedgeDetect()
        {
            bool aboveIsEmpty = !Physics2D.Raycast(
                new Vector2(LedgeSensor.position.x, LedgeSensor.position.y + spanOfLedge),
                Vector2.right * core.Movement.FacingDirection,
                ledgeDistance,
                TargetLayerForLedgeSensor);

            bool betweenHitsIsEmpty = !Physics2D.Raycast(
                LedgeSensor.position,
                Vector2.up,
                spanOfLedge,
                TargetLayerForLedgeSensor);

            return LedgeHit.collider != null
                && (LedgeHit.collider.CompareTag(Platform) || LedgeHit.collider.CompareTag(OneWayPlatform))
                && betweenHitsIsEmpty
                && aboveIsEmpty;
        }

        public bool GetDetectedGrabWallPosition(out Vector2 wallPosition)
        {
            wallPosition = Vector2.zero;
            bool isDetected = IsGrabWallDetect();

            if (isDetected)
            {
                wallPosition = WallHit.point;              
            }

            return isDetected;
        }

        public bool GetDetectedLedgeCorner(out Vector2 ledgeCorner)
        {
            ledgeCorner = Vector2.zero;
            bool isDetected = IsLedgeDetect();
          
            if (isDetected)
            {
                Vector3Int cellPosition = grid.WorldToCell(LedgeHit.point);
                Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);
                ledgeCorner.Set(centerOfCell.x, centerOfCell.y);
            }

            return isDetected;
        }

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
            Gizmos.DrawWireSphere(CeillingSensor.position, cellingRadius); //celling ray 
            Gizmos.DrawRay(GroundSensor.position, new Vector2(0, -groundDistance)); //ground ray

            Gizmos.DrawRay(WallSensor.position, new Vector2(wallDistance * core.Movement.FacingDirection, 0)); //wall ray 1
            Gizmos.DrawRay(new Vector2(WallSensor.position.x, WallSensor.position.y - spanOfGrabWall), new Vector2(wallDistance * core.Movement.FacingDirection, 0)); //wall ray 2

            Gizmos.DrawRay(new Vector2(LedgeSensor.position.x, LedgeSensor.position.y + spanOfLedge), new Vector2(ledgeDistance * core.Movement.FacingDirection, 0)); //ledge ray 1
            Gizmos.DrawRay(LedgeSensor.position, new Vector2(ledgeDistance * core.Movement.FacingDirection, 0)); //ledge ray 2
            Gizmos.DrawRay(LedgeSensor.position, new Vector2(0, spanOfLedge)); //ledge ray between      
        }
    }
}
