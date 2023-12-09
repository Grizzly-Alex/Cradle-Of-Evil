using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerCrouchState : PlayerOnGroundState
    {
        private readonly int hashIdle = Animator.StringToHash("IdleCrouch");
        private bool isTouchedRoof;
		protected override float MoveSpeed => player.Data.CrouchMoveSpeed;

        public PlayerCrouchState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
			
		}

        public override void Enter()
        {
            base.Enter();

            player.Input.SitStandEvent += OnStand;

            player.SetColliderHeight(player.Data.CrouchColiderHeight);
            player.Animator.Play(hashIdle);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.SitStandEvent -= OnStand;
        }

        public override void DoCheck()
        {
            base.DoCheck();
            isTouchedRoof = player.Core.Sensor.IsCellingDetect();
        }

        public override void AnimationTrigger()
        {
            player.Core.VisualFx.CreateDust(
                DustType.Tiny,
                new Vector2()
                {
                    x = player.Core.Movement.FacingDirection != 1 
                        ? player.BodyCollider.bounds.max.x 
                        : player.BodyCollider.bounds.min.x,
                    y = player.Core.Sensor.GroundHit.point.y,
                },
                player.transform.rotation);
        }

        #region Input
        private void OnStand()
        {
            if (!isTouchedRoof) stateMachine.ChangeState(player.States.SitStand);            
        }

		protected override void OnJump()
        {
            if (isTouchedRoof) return; 

            stateMachine.ChangeState(player.States.Jump);

            player.Core.VisualFx.CreateDust(
                DustType.JumpFromGround,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);
        }

		protected override void OnSlide()
        {
            stateMachine.ChangeState(player.States.Slide);

            player.Core.VisualFx.CreateDust(
                DustType.StartSlide,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);
        }
        #endregion
    }
}