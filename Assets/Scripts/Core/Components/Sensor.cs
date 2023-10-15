using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class Sensor : CoreComponent
    {
        [Header("SIZE OF SENSOR")]
        [SerializeField] private float _cellingRadius;
        [SerializeField] private float _groundDistance;
        [SerializeField] private float _wallDistance;

        [field: Header("POSITION OF SENSOR")]
        [field: SerializeField] public Transform CeilingSensor { get; private set; }
        [field: SerializeField] public Transform GroundSensor { get; private set; }
        [field: SerializeField] public Transform WallSensor { get; private set; }
        [field: SerializeField] public Transform LedgeHorizontalSensor { get; private set; }
        [field: SerializeField] public Transform LedgeVerticalSensor { get; private set; }

        [field: Header("LAYER MASK")]
        [field: SerializeField] public LayerMask PlatformsLayer { get; private set; }

        [field: Header("TAG MASK")]
        [field: SerializeField] public string Ground { get; private set; }
        [field: SerializeField] public string GrabWall { get; private set; }

        #region Sensors
        public RaycastHit2D GroundHit => Physics2D.Raycast(
            GroundSensor.position,
            Vector2.down,
            _groundDistance,
            PlatformsLayer);

        public RaycastHit2D WallHit => Physics2D.Raycast(
            WallSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            _wallDistance,
            PlatformsLayer);

        public RaycastHit2D LedgeHorizontalHit => Physics2D.Raycast(
            LedgeHorizontalSensor.position,
            Vector2.right * core.Movement.FacingDirection,
            _wallDistance,
            PlatformsLayer);

        public RaycastHit2D LedgeVerticalHit => Physics2D.Raycast(
            new Vector2(LedgeVerticalSensor.position.x * core.Movement.FacingDirection, LedgeVerticalSensor.position.y),
            Vector2.down,
            _groundDistance,
            PlatformsLayer);

        public Collider2D Circle => Physics2D.OverlapCircle(CeilingSensor.position, _cellingRadius, PlatformsLayer);
        #endregion

        public bool IsWallDetect() => WallHit;
        public bool IsGroundDetect() => GroundHit;
        public bool IsCellingDetect() => Circle;
        public bool IsLedgeVerticalDetect() => !LedgeVerticalHit && GroundHit;     
        public bool IsGrabWallDetect() => WallHit.collider != null && WallHit.collider.CompareTag(GrabWall);
        public bool IsLedgeHorizontalDetect() 
            => !Physics2D.Raycast(LedgeHorizontalSensor.position,
                Vector2.down, LedgeHorizontalSensor.position.y - WallSensor.position.y, PlatformsLayer)
                && !LedgeHorizontalHit && WallHit;
        public float GetGroundSlopeAngle() => Vector2.Angle(GroundHit.normal, Vector2.up);
        public Vector2 GetGroundPerperdicular() => Vector2.Perpendicular(core.Sensor.GroundHit.normal).normalized;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(CeilingSensor.position, _cellingRadius); //celling ray 
            Gizmos.DrawRay(GroundSensor.position, new Vector2(0, - _groundDistance)); //ground ray
            Gizmos.DrawRay(WallSensor.position, new Vector2(- _wallDistance, 0)); //wall ray
            Gizmos.DrawRay(LedgeHorizontalSensor.position, new Vector2(- _wallDistance, 0)); //ledge horizontal ray    
        }
    }

}
