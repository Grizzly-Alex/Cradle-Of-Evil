using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement: CoreComponent
{
    public Rigidbody2D Rigidbody { get; private set; }
    public int FacingDirection { get; private set; }
    private Vector2 workingVector;

    protected override void Awake()
    {
        base.Awake();

        Rigidbody = GetComponentInParent<Rigidbody2D>();
        FacingDirection = 1;
    }

    public void MoveAlongSurface(float velocity)
    {
        if(core.CollisionSenses.GetGroundSlopeAngle() != 0.0f)
        {
            SetVelocityOnSlope(velocity);
        }
        else
        {
            SetVelocityOnSmooth(velocity);
        }
    }

    public void SetVelocityOnSlope(float velocity)
    {
        workingVector.Set(
            -velocity * core.CollisionSenses.GetPerpendicularSurface().x,
            -velocity * core.CollisionSenses.GetPerpendicularSurface().y);

        SetFinalVelocity();
    }

    public void SetVelocityOnSmooth(float velocity)
    {
        workingVector.Set(velocity, Vector2.zero.y);
        SetFinalVelocity();
    }

    public void SetVelocityX(float velocity)
    {
        workingVector.Set(velocity, Rigidbody.velocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        workingVector.Set(Rigidbody.velocity.x, velocity);
        SetFinalVelocity();
    }

    public void SetVelocityZero()
    {
        workingVector = Vector2.zero;        
        SetFinalVelocity();
    }

    public void SetFinalVelocity() => Rigidbody.velocity = workingVector;


    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        Rigidbody.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}