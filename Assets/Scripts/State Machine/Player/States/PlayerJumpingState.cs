using UnityEngine;

public sealed class PlayerJumpingState : PlayerInAirState
{
    private readonly int HashFirstJump = Animator.StringToHash("FirstJump");
    private readonly int HashSecondJump = Animator.StringToHash("SecondJump");
    private int jumpsLeft;
    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        jumpsLeft = playerData.AmountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        jumpsLeft -= 1;
       
        core.Movement.SetVelocityY(playerData.JumpForce);

        switch(jumpsLeft)
        {
            case 1: animator.Play(HashFirstJump); break ;
            case 0: animator.Play(HashSecondJump); break ;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(core.Movement.CurrentVelocity.y <= 0.0f)
        {
            stateMachine.SwitchState(stateMachine.FallingState);
        }
    }

    public bool CanJump() => jumpsLeft > 0;

    public void ResetAmountOfJumpsLeft() => jumpsLeft = playerData.AmountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => jumpsLeft--;
}