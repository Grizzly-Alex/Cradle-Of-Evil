using Entities;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerLandingState : PlayerBaseState
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

            if (LandingForce >= player.Data.LandingThreshold)
            {
                player.Animator.Play(hashHardLanding);
                updateLogic = () =>
                {
                    if (isAnimFinished) 
                        stateMachine.ChangeState(player.StandState);                    
                };                               
            }
            else
            {
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
            if (LandingForce < player.Data.LandingThreshold)
                stateMachine.ChangeState(player.JumpState);
        }
    }
}
