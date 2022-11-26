using UnityEngine;

public sealed class Movement: CoreComponent
{
    [field: SerializeField] public PhysicsMaterial2D Friction {get; private set;}
    [field: SerializeField] public PhysicsMaterial2D NotFriction {get; private set;}
    public Rigidbody2D Rigidbody { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workingVector;
    

    protected override void Awake()
    {
        base.Awake();

        Rigidbody = GetComponentInParent<Rigidbody2D>();
        if(Rigidbody.sharedMaterial is null) { Rigidbody.sharedMaterial = Friction; }
        FacingDirection = 1;
    }

    public void LogicUpdate()
    {
        CurrentVelocity = Rigidbody.velocity;
    }

    public void MoveAlongSurface(float velocity)
    {      
        if(core.CollisionSensors.GroundSlopeAngle != 0.0f)
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
            -velocity * core.CollisionSensors.GroundPerpendicular.x,
            -velocity * core.CollisionSensors.GroundPerpendicular.y);

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

    public void SetFinalVelocity()
    {
        Rigidbody.velocity = workingVector;
        CurrentVelocity = workingVector;
    } 

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        Rigidbody.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void IsFriction(bool isBrake)
    {
        if(isBrake)
        {
            SetFriction(Friction);
        }
        else
        {
            SetFriction(NotFriction);
        }
    }

    public void SwitchFriction(int InputX)
    {
        switch (InputX)
        {
            case 0: SetFriction(Friction); break;
            default: SetFriction(NotFriction); break;
        }
    }

    public void SetFriction(PhysicsMaterial2D frictionMaterial)
    {
        if(frictionMaterial.friction != Rigidbody.sharedMaterial.friction)
        {
            Rigidbody.sharedMaterial = frictionMaterial;
        }
    }   
}