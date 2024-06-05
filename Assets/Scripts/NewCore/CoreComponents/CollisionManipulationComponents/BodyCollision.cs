using NewCoreSystem;
using System;
using UnityEngine;


namespace NewCore.CoreComponents.CollisionManipulationComponents
{
    [Serializable]
    public class BodyCollision : CollisionManipulationComponent
    {
        public BodyCollision(Core core) : base(core)
        {
        }

        public void SetColliderHeight(float height)
        {
            if (entityCollider.size.y == height) return;

            Vector2 offset = entityCollider.offset;
            Vector2 newSize = new(entityCollider.size.x, height);
            offset.y += (height - entityCollider.size.y) / 2;
            entityCollider.size = newSize;
            entityCollider.offset = offset;
        }
    }
}
