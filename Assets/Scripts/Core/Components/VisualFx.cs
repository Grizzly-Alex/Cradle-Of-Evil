using Pool.ContainersPool;
using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class VisualFx : CoreComponent
    {
        private SpriteRenderer spriteRenderer;

        [Header("AFTER IMAGE")]
        [SerializeField] private AfterImageContainer afterImageContainer;
        private float afterImageCooldownTimer;
        //[Range(0, 1)]
        //[SerializeField]
        //private float alphaBegin = 1;
        //[SerializeField]
        //private float colorLooseRate = 1;


        protected override void Awake()
        {
            base.Awake();

        }

        protected override void Start()
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
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
                var obj = afterImageContainer.Pool.Get();
                obj.Release = afterImageContainer.Pool.Release;
            }
        }
    }
}
