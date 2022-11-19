using UnityEngine;

public enum SitOrStandTransition { Standing, Crouching } 

public sealed class PlayerSitOrStandState : PlayerBaseState
{
    private readonly int HashSitDown = Animator.StringToHash("SitDown");
    private readonly int HashStandUp = Animator.StringToHash("StandUp");
    public SitOrStandTransition TransitionTo { get; set; }

    public PlayerSitOrStandState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.IsFriction(true);

        switch (TransitionTo)
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
            switch (TransitionTo)
            {
                case SitOrStandTransition.Crouching:
                    stateMachine.SwitchState(stateMachine.CrouchingState);
                    break;
                case SitOrStandTransition.Standing: 
                    stateMachine.SwitchState(stateMachine.StandingState); 
                    break;
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
}