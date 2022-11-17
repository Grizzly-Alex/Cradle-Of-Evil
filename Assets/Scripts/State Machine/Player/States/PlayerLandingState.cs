using UnityEngine;

public enum LandingStandTransition { HardLanding, Crouching } 

public class PlayerLandingState : PlayerBaseState
{
    private readonly int HashHardLandingState = Animator.StringToHash("HardLanding");
    private readonly int HashSoftLandingState = Animator.StringToHash("SoftLanding");
    public float LandingForce { get; set; }

    public PlayerLandingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.JumpingState.ResetAmountOfJumpsLeft();
        core.Movement.IsFriction(true);
        core.Movement.SetVelocityZero();

        input.JumpEvent += OnJump;

        switch(LandingForce <= playerData.LandingThreshold)
        {
            case true: animator.Play(HashHardLandingState); break;
            case false: animator.Play(HashSoftLandingState); break;
        }       
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(LandingForce <= playerData.LandingThreshold)
        {
            if(isAnimationFinished)
            {
                stateMachine.SwitchState(stateMachine.StandingState);
            }           
        }
        else
        {
            if(isAnimationFinished || input.NormInputX != 0)
            {
                stateMachine.SwitchState(stateMachine.StandingState);
            }  
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();

        input.JumpEvent -= OnJump;
    }

    private void OnJump()
    {
        if(LandingForce > playerData.LandingThreshold)
        {
           stateMachine.SwitchState(stateMachine.JumpingState); 
        }
    } 
}