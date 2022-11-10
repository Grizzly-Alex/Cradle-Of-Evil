using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public sealed class CollisionSenses : CoreComponent
{
    [field: Header("GROUND DETECTION")]
    [field: SerializeField] public LayerMask GroundMask { get; private set; }
    [field: SerializeField] public float GroundRayDistance { get; private set; }
    [field: SerializeField] public float GroundRayYposition { get; private set; }

    [field: Header("ROOF DETECTION")]
    [field: SerializeField] public float RoofRayDistance { get; private set; }

    public CapsuleCollider2D PlayerCollider {get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        PlayerCollider = GetComponentInParent<CapsuleCollider2D>(); 
    }


    public bool DetectingGround() => GetGroundHit();

    public bool DetectingRoof() => GetRoofHits().Any(hit => hit); 

    public float GetGroundSlopeAngle() => Vector2.Angle(GetGroundHit().normal, Vector2.up);
    
    public Vector2 GetPerpendicularSurface() => Vector2.Perpendicular(GetGroundHit().normal).normalized;

    private RaycastHit2D GetGroundHit()
    {
        Vector2 pointHit = new Vector2(PlayerCollider.bounds.center.x, PlayerCollider.bounds.min.y + GroundRayYposition);
        return Physics2D.Raycast(pointHit, Vector2.down, GroundRayDistance, GroundMask);   
    }

    public List<RaycastHit2D> GetRoofHits()
    {               
        return new List<RaycastHit2D>
        {
            Physics2D.Raycast(new Vector2(PlayerCollider.bounds.min.x, PlayerCollider.bounds.max.y),
            Vector2.up, RoofRayDistance, GroundMask),
            Physics2D.Raycast(new Vector2(PlayerCollider.bounds.max.x, PlayerCollider.bounds.max.y),
            Vector2.up, RoofRayDistance, GroundMask)
        };
    } 

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(new Vector2(PlayerCollider.bounds.center.x, PlayerCollider.bounds.min.y), GroundCheckRadius);  
        //Gizmos.DrawRay(new Vector2(PlayerCollider.bounds.center.x, PlayerCollider.bounds.min.y + GroundRayYposition), new Vector2(0, - GroundRayDistance)); //ground ray
        //Gizmos.DrawRay(new Vector2(PlayerCollider.bounds.min.x, PlayerCollider.bounds.max.y), new Vector2(0, GroundRayDistance)); //roof ray
        //Gizmos.DrawRay(new Vector2(PlayerCollider.bounds.max.x, PlayerCollider.bounds.max.y), new Vector2(0, GroundRayDistance)); //roof ray
    }
    
  
}