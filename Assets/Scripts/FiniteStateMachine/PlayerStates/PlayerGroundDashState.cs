using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerGroundDashState : PlayerAbilityState
    {
        private readonly int hashStartDash = Animator.StringToHash("StartDash");
        private readonly int hashDashing = Animator.StringToHash("Dashing");
        private readonly int hashToCrouch = Animator.StringToHash("toCrouch");
        private readonly int hashToStand = Animator.StringToHash("toStand");
        private float finishTime;

        public PlayerGroundDashState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public override void Enter()
        {
            base.Enter();
            finishTime = Time.time + player.GroundDashTime;

            player.Core.Movement.ResetFreezePos();
            player.SetColliderHeight(player.CrouchColiderHeight);

            switch (stateMachine.PreviousState)
            {
                case PlayerCrouchState: player.Animator.Play(hashDashing); break;
                case PlayerStandState: player.Animator.Play(hashStartDash); break;
            }
        }

        public override void Update()
        {
            base.Update();

            if (!isGrounded)
            {
                isAbilityDone = true;
            }
            else if (Time.time >= finishTime)
            {
                if (!isAnimFinished)
                {
                    player.Core.Movement.SetVelocityZero();
                    player.Core.Movement.FreezePosOnSlope();

                    if (!isCellingDetected)
                    {
                        switch (stateMachine.PreviousState)
                        {
                            case PlayerCrouchState: player.Animator.SetTrigger(hashToCrouch); break;
                            case PlayerStandState: player.Animator.SetTrigger(hashToStand); break;
                        }
                    }
                    else player.Animator.SetTrigger(hashToCrouch);
                }
                else isAbilityDone = true;
            }
            else player.Core.Movement.MoveAlongSurface(player.GroundDashSpeed, player.Core.Movement.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();

            player.Animator.ResetTrigger(hashToStand);
            player.Animator.ResetTrigger(hashToCrouch);
            player.Core.Movement.ResetFreezePos();
            player.Input.DashInputCooldown = player.GroundDashCooldown;
        }
    }
}