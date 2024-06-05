using NewCoreSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace NewCore.CoreComponents.CollisionManipulationComponents
{
    [Serializable]
    public class PlatformCollision : CollisionManipulationComponent
    {
        [SerializeField]
        private float timeIgnoreCollisions;
        [SerializeField]
        private TilemapCollider2D oneWayPlatformCollider;


        public PlatformCollision(Core core) : base(core)
        {
        }

        public void LeaveOneWayPlatform()
        {
            IgnoreOneWayPlatform(true);
            core.StartCoroutine(StopIgnoringOneWayPlatform(timeIgnoreCollisions));
        }
        public void IgnoreOneWayPlatform(bool isIgnore)
        {
            if (Physics2D.GetIgnoreCollision(oneWayPlatformCollider, entityCollider) == isIgnore) return;
            Physics2D.IgnoreCollision(oneWayPlatformCollider, entityCollider, isIgnore);
        }

        private IEnumerator StopIgnoringOneWayPlatform(float delay)
        {
            yield return new WaitForSeconds(delay);
            IgnoreOneWayPlatform(false);
        }
    }
}
