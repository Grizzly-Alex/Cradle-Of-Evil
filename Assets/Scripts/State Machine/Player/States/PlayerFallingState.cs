using UnityEngine;

public sealed class PlayerFallingState: PlayerInAirState
{
    private readonly int HashFallingState = Animator.StringToHash("FallingState");
    private float fallingForce;
    private bool isLedgeDetected;
    public bool canGrabLedge = true;
    private float startTime;
    
    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {       
    }

    public override void Enter()
    {
        base.Enter();

        startTime = Time.time;

        animator.Play(HashFallingState);                 
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isLedgeDetected = core.CollisionSensors.ledgeHorizontalDetect;
        
        if (isLedgeDetected)
        {
            stateMachine.LedgeClimbState.DetectedPos = stateMachine.transform.position;
        }
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
        else if (isLedgeDetected && CheckCanGrabLedge())
        {            
            stateMachine.SwitchState(stateMachine.LedgeClimbState);
        }     
    }

    public override void Exit()
    {
        base.Exit();
        
        ResetFallingForce();
        canGrabLedge = true;       
    }

    public void ResetGrabLedge() => canGrabLedge = false;

    private bool CheckCanGrabLedge() => !canGrabLedge ? canGrabLedge = Cooldown(playerData.GrabLedgeCooldown) : true;

    private bool Cooldown(float finishTime) => Time.time >= finishTime + startTime;

    private void ResetFallingForce() => fallingForce = 0.0f;

    private void SetFallingForce(Vector2 velocity)    
    {
        if (fallingForce > velocity.y)
        {            
            fallingForce = velocity.y;
        }
    }
}