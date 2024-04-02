using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerSlideState : PlayerAbilityState
    {
        private readonly int hashStartDash = Animator.StringToHash("StartSlide");
        private readonly int hashIsMoving = Animator.StringToHash("isMoving");

        private bool isMoving;
        private bool isTouchedRoof;
        private float finishTime;
        private Dust dust;

        public PlayerSlideState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public override void Enter()
        {
            base.Enter();

            player.Core.VisualFx.CreateAnimationFX(
                DustType.StartSlide,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);

            dust = (Dust)player.Core.VisualFx.CreateAnimationFX(
                DustType.Sliding,
                player.transform, 
                new Vector2(x: 0.4f, y: 0.0f));

            finishTime = Time.time + player.Data.SlideTime;

            player.Core.Movement.ResetFreezePos();
            player.SetColliderHeight(player.Data.CrouchColiderHeight);

            player.Animator.SetBool(hashIsMoving, true);
            player.Animator.Play(hashStartDash);
            isMoving = true;
            player.Input.InputCooldown = player.Data.SlideCooldown;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isGrounded)
            {
                isMoving = false;
                isAbilityDone = true;
            }
            else if (Time.time >= finishTime && !isTouchedRoof)
            {
                dust.ReturnToPool();
                player.Animator.SetBool(hashIsMoving, false);
                isMoving = false;

                if (!isAnimFinished)
                {
                    player.Core.Movement.SetVelocityZero();
                    player.Core.Movement.FreezePosOnSlope();
                }
                else
                { 
                    isAbilityDone = true;
                }
            }
            else
            {               
                player.Core.VisualFx.CreateAfterImage(distanceBetweenImages: 0.6f);

            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (isMoving)
            {
                player.Core.Movement.MoveAlongSurface(player.Data.SlideSpeed, player.Core.Movement.FacingDirection);
            }
        }

        public override void Exit()
        {
            base.Exit();

            dust.ReturnToPool();
            player.Core.Movement.ResetFreezePos();
        }

        public override void DoCheck()
        {
            base.DoCheck();

            isTouchedRoof = player.Core.Sensor.IsCellingDetect();
        }

        public override void AnimationTrigger()
        {
            float offsetX = 0.4f; 

            player.Core.VisualFx.CreateAnimationFX(
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