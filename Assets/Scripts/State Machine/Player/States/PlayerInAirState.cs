using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    protected readonly int HashVelocityY = Animator.StringToHash("velocityY");
    
    public PlayerInAirState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        input.JumpEvent += OnJump; 

        SetColliderHeight(playerData.StandingColiderHeight);

        core.Movement.IsFriction(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.CheckIfShouldFlip(input.NormInputX);

        animator.SetFloat(HashVelocityY, core.Movement.CurrentVelocity.y);  
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); 

        core.Movement.SetVelocityX(input.NormInputX * playerData.InAirMoveSpeed);      
    }

    public override void Exit()
    {
        base.Exit(); 
         
        input.JumpEvent -= OnJump;
    }

    private void OnJump()
    {  
        if(stateMachine.JumpingState.CanJump())
        {
            stateMachine.SwitchState(stateMachine.JumpingState);
        }      
    } 
}