using UnityEngine;


namespace NewCore.CoreComponents.PhysicsManipulationComponents
{
    public class Freezing : PhysicsManipulationComponent
    {
        public void FreezePosOnSlope()
        {
            if (core.Sensor.GroundDetector.IsGroundSlope())
                FreezePosX();
        }

        public void FreezePosX()
        {
            if (body.constraints is (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX)) return;
            body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        public void FreezePosY()
        {
            if (body.constraints is (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY)) return;
            body.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        public void ResetFreezePos()
        {
            if (body.constraints == RigidbodyConstraints2D.FreezeRotation) return;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}