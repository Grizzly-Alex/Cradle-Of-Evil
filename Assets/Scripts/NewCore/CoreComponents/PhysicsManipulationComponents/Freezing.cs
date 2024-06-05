using NewCoreSystem;
using System;


namespace NewCore.CoreComponents.PhysicsManipulationComponents
{
    [Serializable]
    public class Freezing : PhysicsManipulationComponent
    {
        public Freezing(Core core) : base(core)
        {
        }

        public void FreezePosOnSlope()
        {
            //if (core.Sensor.IsGroundSlope())
                //FreezePosX();
        }

        public void FreezePosX()
        {
            //if (rigidbody.constraints is (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX)) return;
            //rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        public void FreezePosY()
        {
            //if (rigidbody.constraints is (RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY)) return;
            //rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }

        public void ResetFreezePos()
        {
            //if (rigidbody.constraints == RigidbodyConstraints2D.FreezeRotation) return;
            //rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}