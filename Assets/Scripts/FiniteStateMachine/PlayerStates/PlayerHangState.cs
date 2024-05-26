using Entities;
using System.Collections;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerHangState : PlayerState
    {
        private readonly int hashHanging = Animator.StringToHash("Grab");
        protected bool isHanging;
        public bool CanHang { get; private set; } = true;
        public static Vector2 GrapPosition { get; set; }

        public PlayerHangState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
             
        }

        public override void Enter()
        {
            base.Enter();
            player.Animator.Play(hashHanging);

            player.Core.Movement.SetVelocityZero();
            player.Core.Movement.FreezePosY();

            player.transform.position = GetHangingPosition();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.Input.InputVertical == Vector2.down.y && isHanging)
            {
                CanHang = false;
                player.StartCoroutine(ResetCanHang(0.2f));
                player.Core.Movement.SetVelocityY(0.1f);
                stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            isHanging = false;
            player.Core.Movement.ResetFreezePos();               
        }

        public override void AnimationTrigger()
            => isHanging = true;


        public IEnumerator ResetCanHang(float delay)
        {
            yield return new WaitForSeconds(delay);
            CanHang = true;
        }

        protected abstract Vector2 GetHangingPosition();
    }
}
