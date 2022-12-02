using UnityEngine;

public sealed class PlayerFallingState : PlayerInAirState
{
    private readonly int HashFallingState = Animator.StringToHash("FallingState");
    private float fallingForce;
    private bool isLedgeDetected;
    
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        animator.Play(HashFallingState);       
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isLedgeDetected = core.CollisionSensors.ledgeHorizontalDetect;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();  
     
        SetFallingForce(core.Movement.CurrentVelocity);

        if (isGrounded)
        {
            stateMachine.LandingState.LandingForce = fallingForce;
            stateMachine.SwitchState(stateMachine.LandingState);
        }
        else if (isLedgeDetected)
        {
            stateMachine.LedgeClimbState.DetectedPos = stateMachine.transform.position;
            stateMachine.SwitchState(stateMachine.LedgeClimbState);
        }     
    }

    public override void Exit()
    {
        base.Exit();
        
        ResetFallingForce();
    }
 
    private void SetFallingForce(Vector2 velocity)    
    {
        if (fallingForce > velocity.y)
        {            
            fallingForce = velocity.y;
        }
    }

    private void ResetFallingForce() => fallingForce = 0.0f;
}