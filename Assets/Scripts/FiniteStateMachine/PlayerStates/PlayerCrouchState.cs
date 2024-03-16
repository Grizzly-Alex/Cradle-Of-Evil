using Entities;
using Pool.ItemsPool;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerCrouchState : PlayerOnGroundState
    {
        protected override float ColiderHeight => player.Data.CrouchColiderHeight;
        private readonly int hashIdle = Animator.StringToHash("IdleCrouch");
        private bool isTouchedRoof;


        public PlayerCrouchState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
			
		}

        public override void Enter()
        {
            base.Enter();

            player.Input.SitStandEvent += OnStand;
            player.Input.JumpEvent += OnJump;

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
            player.Input.JumpEvent -= OnJump;
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
            if (!isTouchedRoof) stateMachine.ChangeState(player.SitStandState);            
        }

		private void OnJump()
        {
            if (isTouchedRoof) return;

            player.SetColliderHeight(player.Data.StandColiderHeight);

            stateMachine.ChangeState(player.JumpState);

            player.Core.VisualFx.CreateDust(
                DustType.JumpFromGround,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);
        }

		//private void OnSlide()
  //      {
  //          stateMachine.ChangeState(player.SlideState);

  //          player.Core.VisualFx.CreateDust(
  //              DustType.StartSlide,
  //              player.Core.Sensor.GroundHit.point,
  //              player.transform.rotation);
  //      }
        #endregion
    }
}