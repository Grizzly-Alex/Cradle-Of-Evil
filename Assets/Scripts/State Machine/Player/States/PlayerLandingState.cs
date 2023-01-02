using UnityEngine;

public enum LandingStandTransition { HardLanding, Crouching } 

public sealed class PlayerLandingState : PlayerBaseState
{
    public float LandingForce { get; set; }
    private Vector2 _holdPosition;
    private readonly int _hashHardLandingState = Animator.StringToHash("HardLanding");
    private readonly int _hashSoftLandingState = Animator.StringToHash("SoftLanding");

    public PlayerLandingState(PlayerStateMachine playerSm) : base(playerSm)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        playerSm.Core.Movement.SetRbConstraints(RigidbodyConstraints2D.FreezeAll);  

        playerSm.JumpingState.ResetAmountOfJumpsLeft();

        playerSm.Core.Movement.SetVelocityZero();

        playerSm.Input.JumpEvent += OnJump;

        switch(LandingForce <= playerSm.Data.LandingThreshold)
        {
            case true: playerSm.Animator.Play(_hashHardLandingState); break;
            case false: playerSm.Animator.Play(_hashSoftLandingState); break;
        }       
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(LandingForce <= playerSm.Data.LandingThreshold)
        {
            if(_isAnimationFinished)
            {
                playerSm.SwitchState(playerSm.StandingState);
            }           
        }
        else
        {
            if(_isAnimationFinished || playerSm.Input.NormInputX != 0)
            {
                playerSm.SwitchState(playerSm.StandingState);
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

        playerSm.Input.JumpEvent -= OnJump;

        playerSm.Core.Movement.SetRbConstraints(RigidbodyConstraints2D.FreezeRotation); 
    }

    private void OnJump()
    {
        if(LandingForce > playerSm.Data.LandingThreshold)
        {
           playerSm.SwitchState(playerSm.JumpingState); 
        }
    } 
}