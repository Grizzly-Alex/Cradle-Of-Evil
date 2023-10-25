using Assets.Scripts;
using Entities;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerSlideState : PlayerAbilityState
    {
        private readonly int hashStartDash = Animator.StringToHash("StartSlide");
        private readonly int hashDashing = Animator.StringToHash("Sliding");
        private readonly int hashToCrouch = Animator.StringToHash("toCrouch");
        private readonly int hashToStand = Animator.StringToHash("toStand");
        private float finishTime;
        private Action trigger;

        public PlayerSlideState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public override void Enter()
        {
            base.Enter();

            finishTime = Time.time + player.Data.SlideTime;

            player.Core.Movement.ResetFreezePos();
            player.SetColliderHeight(player.Data.CrouchColiderHeight);

            switch (player.PreviousState)
            {
                case PlayerCrouchState: 
                    player.Animator.Play(hashDashing);
                    trigger = () => player.Animator.SetTrigger(hashToCrouch);
                    break;
                case PlayerStandState: 
                    player.Animator.Play(hashStartDash);
                    trigger = () => player.Animator.SetTrigger(hashToStand);
                    break;
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
                        trigger.Invoke();
                    }
                    else player.Animator.SetTrigger(hashToCrouch);
                }
                else isAbilityDone = true;
            }
            else
            {
                AfterImagePoolTest.Instance.GetFromPool();
                player.Core.Movement.MoveAlongSurface(player.Data.SlideSpeed, player.Core.Movement.FacingDirection);
            }
        }

        public override void Exit()
        {
            base.Exit();

            player.Animator.ResetTrigger(hashToStand);
            player.Animator.ResetTrigger(hashToCrouch);
            player.Core.Movement.ResetFreezePos();
            player.Input.DashInputCooldown = player.Data.SlideCooldown;
        }
    }
}