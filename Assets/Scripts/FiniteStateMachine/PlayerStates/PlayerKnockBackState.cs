using Entities;
using Pool.ItemsPool;
using System.Collections;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerKnockBackState : PlayerAbilityState
    {
        private readonly int hashKnockBacking = Animator.StringToHash("KnockBacking");
        private readonly int hashIsMoving = Animator.StringToHash("isMoving");
        private float finishTime;
        private bool isMoving;

        public bool isReady { get; private set; } = true;

        public PlayerKnockBackState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            finishTime = Time.time + player.Data.KnockBackTime;
            player.Core.Movement.ResetFreezePos();
            player.Animator.SetBool(hashIsMoving, true);
            player.Animator.Play(hashKnockBacking);
            isMoving = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(!isGrounded) 
            {
                isMoving = false;
                isAbilityDone = true;
            }
            else if (Time.time >= finishTime)
            {
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
                player.Core.VisualFx.CreateAfterImage(distanceBetweenImages: 0.3f);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (isMoving)
            {
                player.Core.Movement.MoveAlongSurface(player.Data.KnockBackSpeed, player.Core.Movement.FacingDirection * -1);
            }
        }

        public override void Exit()
        {
            base.Exit();

            isReady = false;
            player.StartCoroutine(CoolDown(player.Data.KnockBackCooldown));
            player.Core.Movement.ResetFreezePos();
        }


        public IEnumerator CoolDown(float delay)
        {
            yield return new WaitForSeconds(delay);
            isReady = true;
        }

        public override void AnimationTrigger()
        {
            player.Core.VisualFx.CreateAnimationFX(
                DustType.Brake,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation, 
                flipHorizontal: true);
        }
    }
}
