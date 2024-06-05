using NewCoreSystem;
using System;
using UnityEngine;

namespace NewCore.CoreComponents.PhysicsManipulationComponents
{
    [Serializable]
    public class Movement : PhysicsManipulationComponent
    {
        public Vector2 CurrentVelocity { get; private set; }
        private Vector2 workingVector;

        public Movement(Core core) : base(core)
        {
        }

        public override void LogicUpdate()
        {
            CurrentVelocity = body.velocity;
        }

        #region Movement
        public void Move(float velocity, int directX)
        {
            MoveAlongSurface(velocity, directX);

            if (directX == 0) core.Physics.Freezing.FreezePosOnSlope();
            else core.Physics.Freezing.ResetFreezePos();
        }

        public void MoveAlongSurface(float velocity, int directX)
        {
            if (core.Sensor.GroundDetector.IsGroundSlope()) SetVelocityOnSlope(velocity * directX);
            else SetVelocityOnSmooth(velocity * directX);
        }
        #endregion

        #region Velocity
        public void SetVelocityOnSlope(float velocity)
        {
            Vector2 perperdicular = core.Sensor.GroundDetector.GetGroundPerperdicular();
            workingVector.Set(-velocity * perperdicular.x, -velocity * perperdicular.y);
            SetFinalVelocity();
        }

        public void SetVelocityOnSmooth(float velocity)
        {
            workingVector.Set(velocity, Vector2.zero.y);
            SetFinalVelocity();
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workingVector.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }

        public void SetVelocityX(float velocity)
        {
            workingVector.Set(velocity, CurrentVelocity.y);
            SetFinalVelocity();
        }

        public void SetVelocityY(float velocity)
        {
            workingVector.Set(CurrentVelocity.x, velocity);
            SetFinalVelocity();
        }

        public void SetVelocityZero()
        {
            workingVector = Vector2.zero;
            SetFinalVelocity();
        }

        public void SetFinalVelocity()
        {
            body.velocity = workingVector;
            CurrentVelocity = workingVector;
        }
        #endregion
    }
}
