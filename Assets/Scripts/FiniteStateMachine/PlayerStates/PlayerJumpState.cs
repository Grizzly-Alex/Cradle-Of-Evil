using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerJumpState : PlayerAbilityState
    {
        private readonly int hashDoubleJump = Animator.StringToHash("DoubleJump");
        public int AmountOfJumpsLeft { get; private set; }

        public PlayerJumpState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
            AmountOfJumpsLeft = player.Data.AmountOfJump;
        }

        public override void Enter()
        {
            base.Enter();

            switch (AmountOfJumpsLeft)
            {
                case 2: player.Core.Movement.SetVelocityY(player.Data.FirstJumpForce); break;
                case 1: player.Core.Movement.SetVelocityY(player.Data.SecondJumpForce); break;
            }
        }

        public override void Update()
        {
            base.Update();

            if (!isGrounded) isAbilityDone = true;
        }

        public override void Exit()
        {
            base.Exit();

            AmountOfJumpsLeft--;
        }

        public int GetHashAnimDoubleJump() => hashDoubleJump;
        public bool CanJum() => AmountOfJumpsLeft > 0;
        public void ResetAmountOfJumpsLeft() => AmountOfJumpsLeft = player.Data.AmountOfJump;
        public void DecreaseAmountOfJumpsLeft() => AmountOfJumpsLeft--;
    }
}