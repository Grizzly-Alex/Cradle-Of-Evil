using UnityEngine;

public sealed class PlayerStandingState : PlayerBaseState
{       
    private readonly int HashIdleStand = Animator.StringToHash("IdleStand");
    private readonly int HashRun = Animator.StringToHash("RunStart");
    private readonly int HashisMove = Animator.StringToHash("isMove"); 

    public PlayerStandingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        SetColliderHeight(playerData.StandingColiderHeight);
    
        input.SitStandEvent += OnSit;
        input.JumpEvent += OnJump;
        input.DashEvent += OnDash;

        switch (input.NormInputX != 0)
        {
            case true : animator.Play(HashRun); break; 
            case false: animator.Play(HashIdleStand); break; 
        }
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
      
        core.Movement.MoveAlongSurface(playerData.StandingMoveSpeed * input.NormInputX); 
        core.Movement.SwitchFriction(input.NormInputX);  
    }

    public override void Exit()
    {   
        base.Exit();

        input.SitStandEvent -= OnSit;  
        input.JumpEvent -= OnJump;
        input.DashEvent -= OnDash;
    }

    #region InputMethods
    private void OnJump() => stateMachine.SwitchState(stateMachine.JumpingState);
    
    private void OnSit()
    {
        stateMachine.SitOrStandState.TransitionTo = SitOrStandTransition.Crouching;
        stateMachine.SwitchState(stateMachine.SitOrStandState);
    }

    private void OnDash()
    {
        stateMachine.DashingState.PreviousState = PreviousState.Standing;
        stateMachine.SwitchState(stateMachine.DashingState);
    }        
    #endregion
}