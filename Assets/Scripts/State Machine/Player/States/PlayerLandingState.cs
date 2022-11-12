using UnityEngine;

public class PlayerLandingState : PlayerBaseState
{
    private readonly int HashHardLandingState = Animator.StringToHash("HardLanding");
    private readonly int HashSoftLandingState = Animator.StringToHash("SoftLanding");
    private readonly float LandingForce;

    public PlayerLandingState(PlayerStateMachine stateMachine, float landingForce) : base(stateMachine)
    {
        LandingForce = landingForce;
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
                Debug.Log("HardLanding");
                stateMachine.SwitchState(new PlayerStandingState(stateMachine));
            }           
        }
        else
        {
            if(isAnimationFinished || input.NormInputX != 0)
            {
                Debug.Log("SoftLanding");
                stateMachine.SwitchState(new PlayerStandingState(stateMachine));
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