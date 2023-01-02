using UnityEngine;

public sealed class PlayerJumpingState : PlayerInAirState
{
    private readonly int _hashFirstJump = Animator.StringToHash("FirstJump");
    private readonly int _hashSecondJump = Animator.StringToHash("SecondJump");
    private int jumpsLeft;
    public PlayerJumpingState(PlayerStateMachine playerSm) : base(playerSm)
    {
        jumpsLeft = playerSm.Data.AmountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        jumpsLeft -= 1;
       
        playerSm.Core.Movement.SetVelocityY(playerSm.Data.JumpForce);

        switch(jumpsLeft)
        {
            case 1: playerSm.Animator.Play(_hashFirstJump); break ;
            case 0: playerSm.Animator.Play(_hashSecondJump); break ;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(playerSm.Core.Movement.CurrentVelocity.y <= 0.0f)
        {
            playerSm.SwitchState(playerSm.FallingState);
        }
    }

    public bool CanJump() => jumpsLeft > 0;

    public void ResetAmountOfJumpsLeft() => jumpsLeft = playerSm.Data.AmountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => jumpsLeft--;
}