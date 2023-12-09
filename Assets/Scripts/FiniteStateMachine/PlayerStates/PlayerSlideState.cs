using Entities;
using Pool.ItemsPool;
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
        private Dust dust;

        public PlayerSlideState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public override void Enter()
        {
            base.Enter();

            dust = player.Core.VisualFx.CreateDust(
                DustType.Sliding,
                player.transform, 
                new Vector2(0.4f, 0));

            finishTime = Time.time + player.Data.SlideTime;

            player.Core.Movement.ResetFreezePos();
            player.SetColliderHeight(player.Data.CrouchColiderHeight);
          
            switch (player.States.PreviousState)
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
                dust.ReturnToPool();

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
                player.Core.VisualFx.CreateAfterImage(0.6f);  
                player.Core.Movement.MoveAlongSurface(player.Data.SlideSpeed, player.Core.Movement.FacingDirection);
            }
        }

        public override void Exit()
        {
            base.Exit();

            dust.ReturnToPool();
            player.Animator.ResetTrigger(hashToStand);
            player.Animator.ResetTrigger(hashToCrouch);
            player.Core.Movement.ResetFreezePos();
            player.Input.DashInputCooldown = player.Data.SlideCooldown;
        }

        public override void AnimationTrigger()
        {
            float offsetX = 0.4f; 

            player.Core.VisualFx.CreateDust(
                DustType.Brake,
                new Vector2
                {
                    y = player.BodyCollider.bounds.min.y,
                    x = player.Core.Movement.FacingDirection !=1 
                        ? player.BodyCollider.bounds.min.x - offsetX
                        : player.BodyCollider.bounds.max.x + offsetX,
                },
                player.transform.rotation);
        }
    }
}