using UnityEngine;

public sealed class PlayerCrouchingState : PlayerBaseState
{
    private readonly int HashIdleCrouch = Animator.StringToHash("IdleCrouch");
    private readonly int HashisMove = Animator.StringToHash("isMove"); 

    public PlayerCrouchingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        input.SitStandEvent += OnStand;
        input.JumpEvent += OnJump;

        animator.Play(HashIdleCrouch);  
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        stateMachine.Core.Movement.CheckIfShouldFlip(input.NormInputX);

        animator.SetBool(HashisMove, input.NormInputX != 0 ? true : false);
    
        if(!isGrounded && core.Movement.CurrentVelocity.y < 0.0f)
        {
            SetColliderHeight(playerData.StandingColiderHeight);

            stateMachine.SwitchState(stateMachine.FallingState);        
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
                    
        core.Movement.MoveAlongSurface(playerData.CrouchingMoveSpeed * input.NormInputX);

        SwitchPhysMaterial(input.NormInputX);
    }

    public override void Exit()
    {
        base.Exit();

        input.SitStandEvent -= OnStand;  
        input.JumpEvent -= OnJump;  
    }

    private void OnStand()
    {
        if(!core.CollisionSenses.DetectingRoof())
        {   
            stateMachine.SitOrStandState.SetStateTransitionTo(SitOrStandTransition.Standing);   

            stateMachine.SwitchState(stateMachine.SitOrStandState); 
        }      
    } 

    private void OnJump()
    {
        if(!core.CollisionSenses.DetectingRoof())
        {
            SetColliderHeight(playerData.StandingColiderHeight);
            
            stateMachine.SwitchState(stateMachine.JumpingState);
        }
    } 
}