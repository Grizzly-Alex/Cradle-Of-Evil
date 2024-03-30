using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public class PlayerKnockBackState : PlayerAbilityState
    {
        private readonly int hashKnockBacking = Animator.StringToHash("KnockBacking");
        private readonly int hashIsMoving = Animator.StringToHash("isMoving");
        private float finishTime;
        private bool isMoving;


        public PlayerKnockBackState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.InputCooldown = player.Data.KnockBackCooldown;
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
            player.Core.Movement.ResetFreezePos();
        }

        public override void DoCheck()
        {
            base.DoCheck();
        }

        public override void AnimationTrigger()
        {
            player.Core.VisualFx.CreateDust(
                DustType.Brake,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation, 
                flipHorizontal: true);
        }
    }
}
