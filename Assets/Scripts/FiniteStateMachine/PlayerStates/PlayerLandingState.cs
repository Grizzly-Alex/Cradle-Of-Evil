using Entities;
using Pool.ItemsPool;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerLandingState : PlayerState
	{
        private readonly int hashSoftLanding = Animator.StringToHash("SoftLanding");
        private readonly int hashHardLanding = Animator.StringToHash("HardLanding");

        private bool isGrounded;
        private float landingForce;
        private Action updateLogic;

        public float LandingForce
        {
            get => Mathf.Abs(landingForce); 
            set => landingForce = value; 
        }

        public PlayerLandingState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
           
            player.AirDashState.ResetAmountOfDash();
			player.JumpState.ResetAmountOfJump();
			player.Core.Movement.SetVelocityZero();

            if (player.Core.Sensor.GetGroundSlopeAngle() != default)
            {
                player.Core.Movement.FreezePosX();
            }

            Vector2 groundSurfacePoint = player.Core.Sensor.GroundHit.point;
            Quaternion rotation = player.transform.rotation;

            if (LandingForce >= player.Data.LandingThreshold)
            {
                player.Core.VisualFx.CreateAnimationFX(DustType.HardLanding, groundSurfacePoint, rotation);
                player.Animator.Play(hashHardLanding);
                updateLogic = () =>
                {
                    if (isAnimFinished) 
                        stateMachine.ChangeState(player.StandState);                    
                };                               
            }
            else
            {
                player.Input.JumpEvent += OnJump;
                player.Core.VisualFx.CreateAnimationFX(DustType.Landing, groundSurfacePoint, rotation);
                player.Animator.Play(hashSoftLanding);
                updateLogic = () =>
                {
                    if (isAnimFinished || player.Input.InputHorizontal != default) 
                        stateMachine.ChangeState(player.StandState);
                };
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isGrounded)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else
            {
                updateLogic.Invoke();
            }
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.JumpEvent -= OnJump;
        }     

        public override void DoCheck()
        {
            isGrounded = player.Core.Sensor.IsPlatformDetect()
                || player.Core.Sensor.IsOneWayPlatformDetect();
        }

        #region Input
        private void OnJump()
        {
            if (player.Input.InputVertical == Vector2.down.y)
            {
                if (!player.Core.Sensor.IsOneWayPlatformDetect()) return;
                player.Core.Movement.LeaveOneWayPlatform();
            }
            else
            {
                stateMachine.ChangeState(player.JumpState);
            }
        }
        #endregion
    }
}
