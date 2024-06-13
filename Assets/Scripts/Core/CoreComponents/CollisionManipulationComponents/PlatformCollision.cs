using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace CoreSystem.CoreComponents.CollisionManipulationComponents
{
    public class PlatformCollision : CollisionManipulationComponent
    {
        [SerializeField]
        private float timeIgnoreCollisions;
        [SerializeField]
        private TilemapCollider2D oneWayPlatformCollider;


        public void IgnoreOneWayPlatform()
        {
            IgnoreCollision(true, oneWayPlatformCollider);
            core.StartCoroutine(
                ResetIgnoreCollision(timeIgnoreCollisions, () => IgnoreCollision(false, oneWayPlatformCollider)));
        }

        public void IgnoreCollision(bool isIgnore, Collider2D collider)
        {
            if (Physics2D.GetIgnoreCollision(collider, entityCollider) == isIgnore) return;
            Physics2D.IgnoreCollision(collider, entityCollider, isIgnore);
        }

        private IEnumerator ResetIgnoreCollision(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}
