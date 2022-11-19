using UnityEngine;

public sealed class PlayerCrouchingState : PlayerBaseState
{
    private readonly int HashIdleCrouch = Animator.StringToHash("IdleCrouch");
    private readonly int HashisMove = Animator.StringToHash("isMove"); 
    private bool isRoofDetected;

    public PlayerCrouchingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        input.SitStandEvent += OnStand;
        input.JumpEvent += OnJump;
        input.DashEvent += OnDash;

        animator.Play(HashIdleCrouch);  
    }

    public override void DoCheck()
    {
        base.DoCheck();

        SetColliderHeight(playerData.CrouchingColiderHeight);

        isRoofDetected = core.CollisionSenses.DetectingRoof();
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        stateMachine.Core.Movement.CheckIfShouldFlip(input.NormInputX);

        animator.SetBool(HashisMove, input.NormInputX != 0 ? true : false);
    
        if(!isGrounded && core.Movement.CurrentVelocity.y < 0.0f)
        {
            stateMachine.JumpingState.DecreaseAmountOfJumpsLeft();
            
            stateMachine.SwitchState(stateMachine.FallingState);        
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
                    
        core.Movement.MoveAlongSurface(playerData.CrouchingMoveSpeed * input.NormInputX);

        core.Movement.SwitchFriction(input.NormInputX);
    }

    public override void Exit()
    {
        base.Exit();

        input.SitStandEvent -= OnStand;  
        input.JumpEvent -= OnJump;  
        input.DashEvent -= OnDash;
    }

    #region InputMethods
    private void OnStand()
    {
        if(!isRoofDetected)
        {   
            stateMachine.SitOrStandState.TransitionTo = SitOrStandTransition.Standing;  
            stateMachine.SwitchState(stateMachine.SitOrStandState); 
        }      
    } 

    private void OnJump()
    {
        if(!isRoofDetected)
        {          
            stateMachine.SwitchState(stateMachine.JumpingState);
        }
    } 

    private void OnDash()
    {
        stateMachine.DashingState.PreviousState = PreviousState.Crouching;
        stateMachine.SwitchState(stateMachine.DashingState);
    }        
    #endregion
}