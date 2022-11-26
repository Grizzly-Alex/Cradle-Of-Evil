using UnityEngine;

public sealed class CollisionSensors : CoreComponent
{
    [Header("POSITION OF SENSOR")]
    [SerializeField] private Transform ceilingSensor;
    [SerializeField] private Transform groundSensor;
    [SerializeField] private Transform wallSensor;
    [SerializeField] private Transform ledgeSensorHorizontal;
    [SerializeField] private Transform ledgeSensorVertical;

    [Header("SIZE OF SENSOR")]
    [SerializeField] private float cellingSensorRadius;
    [SerializeField] private float groundSensorDistance;
    [SerializeField] private float wallSensorDistance;

    [field: Header("LAYER MASK")]
    [field: SerializeField] public LayerMask PlatformsLayer { get; private set; }

    [field: Header("TAG MASK")]
    [field: SerializeField] public string Ground { get; private set; }
    [field: SerializeField] public string GrabWall { get; private set; }


    public RaycastHit2D GroundHit => Physics2D.Raycast(groundSensor.position, Vector2.down, Mathf.Infinity, PlatformsLayer);
    public RaycastHit2D WallHit => Physics2D.Raycast(wallSensor.position, Vector2.right * core.Movement.FacingDirection, wallSensorDistance, PlatformsLayer);
    
    public bool GrabWallDetect
    {
        get => WallHit.collider is null ? false : WallHit.collider.CompareTag(GrabWall);
    }

    public bool WallDetect
    {
        get => WallHit.collider is null ? false : WallHit.collider.CompareTag(Ground);
    }

    public bool CellingDetect
    {
        get => Physics2D.OverlapCircle(ceilingSensor.position, cellingSensorRadius, PlatformsLayer);
    }

    public bool GroundDetect
    { 
        get => GroundHit.distance <= groundSensorDistance; 
    }

    public float GroundSlopeAngle
    {
        get => Vector2.Angle(GroundHit.normal, Vector2.up);
    }

    public Vector2 GroundPerpendicular
    {
        get => Vector2.Perpendicular(core.CollisionSensors.GroundHit.normal).normalized;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ceilingSensor.position, cellingSensorRadius); //celling ray 
        Gizmos.DrawRay(groundSensor.position, new Vector2(0, - groundSensorDistance)); //ground ray
        //Gizmos.DrawRay(wallSensor.position, new Vector2(wallSensorDistance * core.Movement.FacingDirection, 0)); //wall ray
        //Gizmos.DrawRay(new Vector2(Collider.bounds.center.x, Collider.bounds.max.y + LedgeRayYposition), new Vector2(LedgeRayDistance * core.Movement.FacingDirection, 0)); //ledge ray1     
    }       
}