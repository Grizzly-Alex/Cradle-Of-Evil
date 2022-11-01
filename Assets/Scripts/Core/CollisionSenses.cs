using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CollisionSenses : CoreComponent
{
    [field: Header("GROUND DETECTION")]
    [field: SerializeField] public bool IsGrounded { get; private set; }
    [field: SerializeField] public float GroundDistance { get; private set; }
    [field: SerializeField] public LayerMask GroundMask { get; private set; }

    public BoxCollider2D BoxCollider {get; private set; }

    private List<RaycastHit2D> groundHits;


    protected override void Awake()
    {
        base.Awake();

        BoxCollider = GetComponentInParent<BoxCollider2D>();       
    }

    

    public bool DetectingGround()
    {
        SetGroundHitsPositions();

        return IsGrounded = groundHits.Any(hit => hit);
    } 


    private void SetGroundHitsPositions()
    {
        float pointHitPosY = (BoxCollider.transform.position.y + BoxCollider.offset.y) - BoxCollider.size.y / 2;
        Vector2 pointHitLeft = new Vector2(BoxCollider.transform.position.x - BoxCollider.size.x / 2, pointHitPosY);
        Vector2 pointHitRight = new Vector2(BoxCollider.transform.position.x + BoxCollider.size.x / 2, pointHitPosY);

        groundHits = new List<RaycastHit2D>()
        {
            Physics2D.Raycast(pointHitLeft, Vector2.down, GroundDistance, GroundMask),
            Physics2D.Raycast(pointHitRight, Vector2.down, GroundDistance, GroundMask)
        };

        Debug.DrawRay(pointHitLeft, Vector2.down * GroundDistance, Color.green);   
        Debug.DrawRay(pointHitRight, Vector2.down * GroundDistance, Color.green);
    }   
}