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

        SetPhysMaterial(materialsData.Friction);
        core.Movement.SetVelocityZero();

        if(LandingForce <= playerData.LandingThreshold)
        {
            animator.Play(HashHardLandingState);
        }
        else
        {
            animator.Play(HashSoftLandingState);
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
    }
}