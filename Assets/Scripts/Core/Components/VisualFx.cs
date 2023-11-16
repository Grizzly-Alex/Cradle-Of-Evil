using Pool;
using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class VisualFx : CoreComponent
    {
        [Header("AFTER IMAGE")]
        [SerializeField] 
        private GameObject afterImagePrefab;
        [SerializeField]
        private int afterImagePreload;
        private float afterImageCooldownTimer;



        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            PoolManger.Instance.CreatePool(afterImagePrefab, afterImagePreload);
        }

        public override void LogicUpdate() 
        { 
            afterImageCooldownTimer -= Time.deltaTime;
        }

        public void CreateAfterImage(float cooldown)
        {
            if (afterImageCooldownTimer < 0)
            {
                afterImageCooldownTimer = cooldown;
                PoolManger.Instance.GetFromPool(afterImagePrefab, transform.position, transform.rotation);
            }
        }
    }
}