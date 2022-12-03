using UnityEngine;

public sealed class CollisionSensors : CoreComponent
{
    [Header("SIZE OF SENSOR")]
    [SerializeField] private float cellingRadius;
    [SerializeField] private float groundDistance;
    [SerializeField] private float wallDistance;
    
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

    #region Raycasts
    public RaycastHit2D GroundHit => Physics2D.Raycast(GroundSensor.position, Vector2.down, groundDistance, PlatformsLayer);
    public RaycastHit2D WallHit => Physics2D.Raycast(WallSensor.position, Vector2.right * core.Movement.FacingDirection, wallDistance, PlatformsLayer);
    public RaycastHit2D ledgeHorizontalHit => Physics2D.Raycast(LedgeHorizontalSensor.position, Vector2.right * core.Movement.FacingDirection, wallDistance, PlatformsLayer); 
    public RaycastHit2D ledgeVerticalHit => Physics2D.Raycast(new Vector2(LedgeVerticalSensor.position.x * core.Movement.FacingDirection, LedgeVerticalSensor.position.y), Vector2.down, groundDistance, PlatformsLayer); 
    #endregion

    #region Bool
    public bool ledgeVerticalDetect => !ledgeVerticalHit && GroundDetect; 
    public bool ledgeHorizontalDetect => !ledgeHorizontalHit && WallDetect; 
    public bool WallDetect => WallHit; 
    public bool GrabWallDetect => WallHit.collider is null ? false : WallHit.collider.CompareTag(GrabWall); 
    public bool GroundDetect => GroundHit; 
    public bool CellingDetect => Physics2D.OverlapCircle(CeilingSensor.position, cellingRadius, PlatformsLayer);        
    #endregion

    #region Other
    public float GroundSlopeAngle => Vector2.Angle(GroundHit.normal, Vector2.up); 
    public Vector2 GroundPerpendicular => Vector2.Perpendicular(core.CollisionSensors.GroundHit.normal).normalized;       
    #endregion



    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CeilingSensor.position, cellingRadius); //celling ray 
        Gizmos.DrawRay(GroundSensor.position, new Vector2(0, - groundDistance)); //ground ray
        Gizmos.DrawRay(WallSensor.position, new Vector2(-wallDistance, 0)); //wall ray
        Gizmos.DrawRay(LedgeHorizontalSensor.position, new Vector2(-wallDistance, 0)); //ledge horizontal ray    
    }       
}