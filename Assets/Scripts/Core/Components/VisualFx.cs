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

        [Header("DUST")]
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
                AfterImageSprite afterImage = PoolManager.Instance
                    .GetFromPool<AfterImageSprite>(afterImagePrefab.gameObject, transform.position, transform.rotation);

                if (afterImage != null)
                {
                    afterImage.Initialize(spriteRenderer, alphaBegin, colorLooseRate);
                    afterImage.SetActive(true);

                    lastImageXpos = transform.position.x;
                }
            }
        }

        public void CreateDust(DustType dustType, Vector2 position, Quaternion rotation, bool flipHorizontal = false)
        {
            Dust dust = PoolManager.Instance.GetFromPool<Dust>(dustPrefab.gameObject, position, rotation);

            if (flipHorizontal)
            {
                dust.transform.Rotate(0.0f, 180, 0.0f);
            }

            if (dust != null)
            {
                dust.Initialize(dustType);
                dust.SetActive(true);
            }
        }

        public Dust CreateDust(DustType dustType, Transform transform, Vector2 offset)
        {
            Dust dust = PoolManager.Instance.GetFromPool<Dust>(dustPrefab.gameObject, transform.position, transform.rotation);

            if (dust != null)
            {
                dust.Initialize(dustType, transform, offset);
                dust.SetActive(true);
            }
            return dust;
        }
    }
}