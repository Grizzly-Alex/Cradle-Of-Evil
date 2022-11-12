using UnityEngine;

public class PlayerJumpingState : PlayerInAirState
{
    private readonly int HashJumpingState = Animator.StringToHash("JumpingState");

    private int jumpsLeft;
    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        jumpsLeft = playerData.JumpCount;
    }

    public override void Enter()
    {
        base.Enter();

        animator.Play(HashJumpingState);
        
        core.Movement.SetVelocityY(playerData.JumpForce);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(core.Movement.CurrentVelocity.y <= 0.0f)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }
    } 

}