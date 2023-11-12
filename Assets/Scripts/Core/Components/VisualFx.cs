using Pool;
using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class VisualFx : CoreComponent
    {
        private SpriteRenderer _masterSpriteRenderer;
        private Transform _masterTransform;


        [Header("AFTER IMAGE")]
        [SerializeField] private GameObject afterImagePrefab;
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
            _masterSpriteRenderer = GetComponentInParent<SpriteRenderer>();
            _masterTransform = GetComponentInParent<Transform>();
            PoolManger.Instance.CreatePool(afterImagePrefab, 4);
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
