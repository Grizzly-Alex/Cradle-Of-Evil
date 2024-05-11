using UnityEngine;

namespace Pool.ItemsPool
{
    public sealed class Shadow : PooledObject
    {
        [SerializeField]
        private SpriteRenderer entitySpriteRenderer;
        private SpriteRenderer spriteRenderer;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();  
        }

        private void Update()
        {
            if (entitySpriteRenderer is not null)
            {
                spriteRenderer.sprite = entitySpriteRenderer.sprite;
            }
        }

        public override GameObject Create(Transform container)
        {
            gameObject.SetActive(true);
            return base.Create(container);
        }

        public void Initialize(SpriteRenderer spriteRendarer)
        {
            entitySpriteRenderer = spriteRendarer;
        }
    }
}
