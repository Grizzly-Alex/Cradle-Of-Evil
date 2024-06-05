using NewCoreSystem;
using System;
using UnityEngine;

namespace NewCore.CoreComponents.PhysicsManipulationComponents
{
    [Serializable]
    public class Flipping : PhysicsManipulationComponent
    {
        public int FacingDirection { get; private set; }

        public Flipping(Core core) : base(core)
        {
            FacingDirection = 1;
        }

        public void FlipToDirection(int xInput)
        {
            if (xInput != Vector2Int.zero.x && xInput != FacingDirection)
                Flip();
        }

        public void Flip()
        {
            FacingDirection *= -1;
            body.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
}
