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

            player.Input.JumpEvent += OnJump;

            player.DashState.ResetAmountOfDash();
			player.JumpState.ResetAmountOfJump();
			player.Core.Movement.SetVelocityZero();

            if (player.Core.Sensor.GetGroundSlopeAngle() != 0.0f)
            {
                player.Core.Movement.FreezePosX();
            }

            Vector2 groundSurfacePoint = player.Core.Sensor.GroundHit.point;
            Quaternion rotation = player.transform.rotation;

            if (LandingForce >= player.Data.LandingThreshold)
            {
                player.Core.VisualFx.CreateDust(DustType.HardLanding, groundSurfacePoint, rotation);
                player.Animator.Play(hashHardLanding);
                updateLogic = () =>
                {
                    if (isAnimFinished) 
                        stateMachine.ChangeState(player.StandState);                    
                };                               
            }
            else
            {
                player.Core.VisualFx.CreateDust(DustType.Landing, groundSurfacePoint, rotation);
                player.Animator.Play(hashSoftLanding);
                updateLogic = () =>
                {
                    if (isAnimFinished || player.Input.NormInputX != 0) 
                        stateMachine.ChangeState(player.StandState);
                };
            }
        }

        public override void Update()
        {
            base.Update();

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
            isGrounded = player.Core.Sensor.IsGroundDetect();
        }
       
        private void OnJump()
        {
            if (LandingForce >= player.Data.LandingThreshold) return;

            stateMachine.ChangeState(player.JumpState);

            player.Core.VisualFx.CreateDust(
                DustType.JumpFromGround,
                player.Core.Sensor.GroundHit.point,
                player.transform.rotation);
        }
    }
}
