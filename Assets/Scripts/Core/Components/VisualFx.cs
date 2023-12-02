using Pool;
using Pool.ItemsPool;
using UnityEngine;

namespace CoreSystem.Components
{
    public sealed class VisualFx : CoreComponent
    {
        private SpriteRenderer spriteRenderer;

        [Header("AFTER IMAGE")]
        [SerializeField] 
        private AfterImageSprite afterImagePrefab;
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin;
        [SerializeField]
        private float colorLooseRate;
        private float lastImageXpos;

        [Header("AFTER IMAGE")]
        [SerializeField]
        private Dust dustPrefab;


        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }

        public void CreateAfterImage(float distanceBetweenImages)
        {
            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                AfterImageSprite afterImage = PoolManger.Instance.GetFromPool<AfterImageSprite>(afterImagePrefab.gameObject, transform.position, transform.rotation);

                if (afterImage != null)
                {
                    afterImage.Initialize(spriteRenderer, alphaBegin, colorLooseRate);
                    afterImage.SetActive(true);

                    lastImageXpos = transform.position.x;
                }
            }
        }

        public void CreateDust(DustType dustType, Vector2 position, Quaternion rotation)
        {
            Dust dust = PoolManger.Instance.GetFromPool<Dust>(dustPrefab.gameObject, position, rotation);

            if (dust != null)
            {
                dust.Initialize(dustType);
                dust.SetActive(true);
            }
        }
    }
}