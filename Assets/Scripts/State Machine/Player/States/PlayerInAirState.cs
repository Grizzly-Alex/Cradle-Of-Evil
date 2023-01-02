using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    protected readonly int _hashVelocityY = Animator.StringToHash("velocityY");
    
    public PlayerInAirState(PlayerStateMachine playerSm) : base(playerSm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        playerSm.Input.JumpEvent += OnJump; 

        SetColliderHeight(playerSm.Data.StandingColiderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        playerSm.Core.Movement.CheckIfShouldFlip(playerSm.Input.NormInputX);

        playerSm.Animator.SetFloat(_hashVelocityY, playerSm.Core.Movement.CurrentVelocity.y);  
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate(); 

        playerSm.Core.Movement.SetVelocityX(playerSm.Input.NormInputX * playerSm.Data.InAirMoveSpeed);      
    }

    public override void Exit()
    {
        base.Exit(); 
         
        playerSm.Input.JumpEvent -= OnJump;
    }

    private void OnJump()
    {  
        if(playerSm.JumpingState.CanJump())
        {
            playerSm.SwitchState(playerSm.JumpingState);
        }      
    } 
}