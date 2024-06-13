using UnityEngine;


namespace CoreSystem.CoreComponents.CollisionManipulationComponents
{
    public class BodyCollision : CollisionManipulationComponent
    {
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
