using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CollisionSenses : CoreComponent
{
    [field: Header("GROUND DETECTION")]
    [field: SerializeField] public LayerMask GroundMask { get; private set; }
    [field: SerializeField] public float GroundRayDistance { get; private set; }

    public CapsuleCollider2D PlayerCollider {get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        PlayerCollider = GetComponentInParent<CapsuleCollider2D>(); 
    }
   
    public bool DetectingGround() => GetGroundHit();  
    
    public float GetGroundSlopeAngle() => Vector2.Angle(GetGroundHit().normal, Vector2.up);

    public Vector2 GetPerpendicularSurface() => Vector2.Perpendicular(GetGroundHit().normal).normalized;

    private RaycastHit2D GetGroundHit()
    {
        Vector2 pointHit = new Vector2(PlayerCollider.bounds.center.x, PlayerCollider.bounds.min.y);
        Debug.DrawRay(pointHit, Vector2.down * GroundRayDistance, Color.red); 
        return Physics2D.Raycast(pointHit, Vector2.down, GroundRayDistance, GroundMask);   
    }       
}