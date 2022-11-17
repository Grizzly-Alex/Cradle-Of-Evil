using UnityEngine;

public enum PreviousState { Standing, Crouching } 

public class PlayerDashingState : PlayerBaseState
{
    private readonly int HashStartDash = Animator.StringToHash("StartDash");
    private readonly int HashDashing = Animator.StringToHash("Dashing");
    private readonly int HashToStand = Animator.StringToHash("toStand");
    private readonly int HashToCrouch = Animator.StringToHash("toCrouch");
    public PreviousState PreviousState { get; set; }
    private float startTime;
    private bool timeOut;
    private bool isRoofDetected;

    public PlayerDashingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        timeOut = false;
        startTime = Time.time;       
        core.Movement.IsFriction(false); 
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

        isRoofDetected = core.CollisionSenses.DetectingRoof();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isGrounded)
        {
            stateMachine.SwitchState(stateMachine.FallingState);
        }

        if (Time.time >= startTime + playerData.DashingTime)
        {           
            timeOut = true; 

            if (isRoofDetected)
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
                            SetColliderHeight(playerData.StandingColiderHeight);
                            stateMachine.SwitchState(stateMachine.StandingState);
                            break;

                        case PreviousState.Crouching: 
                            SetColliderHeight(playerData.CrouchingColiderHeight); 
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
            core.Movement.IsFriction(true);
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