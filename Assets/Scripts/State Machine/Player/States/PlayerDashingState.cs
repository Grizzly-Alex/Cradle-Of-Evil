using UnityEngine;

public enum PreviousState { Standing, Crouching } 

public sealed class PlayerDashingState : PlayerBaseState
{
    private readonly int HashStartDash = Animator.StringToHash("StartDash");
    private readonly int HashDashing = Animator.StringToHash("Dashing");
    private readonly int HashToStand = Animator.StringToHash("toStand");
    private readonly int HashToCrouch = Animator.StringToHash("toCrouch");
    public PreviousState PreviousState { get; set; }
    private float startTime;
    private bool timeOut;
    private bool isCellingDetected;

    public PlayerDashingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
 
        timeOut = false;
        startTime = Time.time;

        SetColliderHeight(playerData.CrouchingColiderHeight);

        switch (PreviousState)
        {
            case PreviousState.Standing: animator.Play(HashStartDash); break;
            case PreviousState.Crouching: animator.Play(HashDashing); break;
        }
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isCellingDetected = core.CollisionSensors.CellingDetect;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isGrounded)
        {
            stateMachine.SwitchState(stateMachine.FallingState);
        }

        if (Time.fixedTime >= startTime + playerData.DashingTime)
        {         
            timeOut = true; 

            if (isCellingDetected)
            {
                animator.SetTrigger(HashToCrouch);
               
                if (isAnimationFinished)
                {
                    stateMachine.SwitchState(stateMachine.CrouchingState);
                }                   
            }
            else 
            {             
                switch (PreviousState)
                {
                    case PreviousState.Standing: animator.SetTrigger(HashToStand); break;
                    case PreviousState.Crouching: animator.SetTrigger(HashToCrouch); break;
                }                 
                
                if (isAnimationFinished)
                {
                    switch (PreviousState)
                    {
                        case PreviousState.Standing:
                            stateMachine.SwitchState(stateMachine.StandingState);
                            break;
                        case PreviousState.Crouching: 
                            stateMachine.SwitchState(stateMachine.CrouchingState); 
                            break;
                    }               
                }                
            }             
        }           
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!timeOut)
        {
            core.Movement.MoveAlongSurface(playerData.DashingSpeed * core.Movement.FacingDirection);
        }
        else
        {
            core.Movement.SetVelocityZero();
        }
    }

    public override void Exit()
    {
        base.Exit(); 

        animator.ResetTrigger(HashToStand);
        animator.ResetTrigger(HashToCrouch);

        input.DashInputCooldown = playerData.DashingCooldown;          
    }
}