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

        input.SitStandEvent += OnSit;
        input.JumpEvent += OnJump;

        if(input.NormInputX != 0)
        {
            animator.Play(HashRun);
        }
        else
        {
            animator.Play(HashIdleStand);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        stateMachine.Core.Movement.CheckIfShouldFlip(input.NormInputX);

        animator.SetBool(HashisMove, input.NormInputX != 0 ? true : false);

        if(!isGrounded && core.Movement.CurrentVelocity.y < 0.0f)
        {
            stateMachine.SwitchState(stateMachine.FallingState);        
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); 
      
        core.Movement.MoveAlongSurface(playerData.StandingMoveSpeed * input.NormInputX);

        SwitchPhysMaterial(input.NormInputX);
    }

    public override void Exit()
    {   
        base.Exit();

        input.SitStandEvent -= OnSit;  
        input.JumpEvent -= OnJump;
    }

    private void OnSit()
    {
        stateMachine.SitOrStandState.SetStateTransitionTo(SitOrStandTransition.Crouching);
        stateMachine.SwitchState(stateMachine.SitOrStandState);
    }

    private void OnJump() => stateMachine.SwitchState(stateMachine.JumpingState);
}