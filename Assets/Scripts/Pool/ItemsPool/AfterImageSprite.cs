using System;
using UnityEngine;

namespace Pool.ItemsPool
{
    public class AfterImageSprite : PoolObject
    {
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin = 1;
        private float alphaUpdate;
        [SerializeField]
        private float colorLooseRate = 1;
        [SerializeField]
        private string tagMask;


        private Transform entity;
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer entitySpriteRenderer;
        private Color color;


        private void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            entity = GameObject.FindGameObjectWithTag(tagMask).transform;
            entitySpriteRenderer = entity.GetComponent<SpriteRenderer>();
            alphaUpdate = alphaBegin;
            color = spriteRenderer.color;
            spriteRenderer.sprite = entitySpriteRenderer.sprite;
        }

        private void Update()
        {
            alphaUpdate -= colorLooseRate * Time.deltaTime;
            spriteRenderer.color = new Color(color.r, color.g, color.b, alphaUpdate);
            if (spriteRenderer.color.a <= 0)
            {
                PoolManger.Instance.ReturnToPool(gameObject);
            }
        }

        public override GameObject Create(Transform container)
        {
            gameObject.SetActive(false);
            return base.Create(container);
        }
    }
}