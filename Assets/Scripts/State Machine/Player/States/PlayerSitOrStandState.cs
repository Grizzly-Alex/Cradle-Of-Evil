using UnityEngine;

public sealed class PlayerSitOrStandState : PlayerBaseState
{
    private readonly int HashSitDown = Animator.StringToHash("SitDown");
    private readonly int HashStandUp = Animator.StringToHash("StandUp");
    private bool isTransitToCrouch;


    public PlayerSitOrStandState(PlayerStateMachine stateMachine, bool isTransitToCrouch) : base(stateMachine)
    {
        this.isTransitToCrouch = isTransitToCrouch;
    }

    public override void Enter()
    {
        base.Enter();

        SetPhysMaterial(materialsData.Friction);
        
        if(isTransitToCrouch)
        {
            animator.Play(HashSitDown);
        }
        else
        {
            animator.Play(HashStandUp);
        }      
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(isTransitToCrouch)
            {
                SetColliderHeight(playerData.CrouchingColiderHeight);

                stateMachine.SwitchState(new PlayerCrouchingState(stateMachine));
            }
            else
            {
                SetColliderHeight(playerData.StandingColiderHeight);
                
                stateMachine.SwitchState(new PlayerStandingState(stateMachine));
            }            
        }

        if(!isGrounded && core.Movement.CurrentVelocity.y < 0.0f)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));        
        }
    }

    public override void PhysicsUpdate()
    {  
        base.PhysicsUpdate();
              
        core.Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
   
    }
}
