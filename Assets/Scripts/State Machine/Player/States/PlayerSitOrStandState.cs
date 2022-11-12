using UnityEngine;

public enum SitOrStandTransition { Standing, Crouching } 

public sealed class PlayerSitOrStandState : PlayerBaseState
{
    private readonly int HashSitDown = Animator.StringToHash("SitDown");
    private readonly int HashStandUp = Animator.StringToHash("StandUp");
    private SitOrStandTransition transitionTo; 

    public PlayerSitOrStandState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        SetPhysMaterial(materialsData.Friction);

        switch (transitionTo)
        {
            case SitOrStandTransition.Crouching: animator.Play(HashSitDown); break;
            case SitOrStandTransition.Standing: animator.Play(HashStandUp); break;
        }           
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (transitionTo == SitOrStandTransition.Crouching)
            {
                SetColliderHeight(playerData.CrouchingColiderHeight);

                stateMachine.SwitchState(stateMachine.CrouchingState);
            }
            else if (transitionTo == SitOrStandTransition.Standing)
            {
                SetColliderHeight(playerData.StandingColiderHeight);
                
                stateMachine.SwitchState(stateMachine.StandingState);
            }            
        }

        if(!isGrounded && core.Movement.CurrentVelocity.y < 0.0f)
        { 
            stateMachine.JumpingState.DecreaseAmountOfJumpsLeft();
            
            stateMachine.SwitchState(stateMachine.FallingState);        
        }
    }

    public override void PhysicsUpdate()
    {  
        base.PhysicsUpdate();
              
        core.Movement.SetVelocityZero();
    }

    public void SetStateTransitionTo(SitOrStandTransition state) => transitionTo = state;
}
