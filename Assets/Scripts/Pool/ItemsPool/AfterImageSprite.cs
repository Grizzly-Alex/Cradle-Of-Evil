using Entities;
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


        private SpriteRenderer spriteRenderer;
        private SpriteRenderer entitySpriteRenderer;
        private Color color;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            entitySpriteRenderer = GameObject.FindGameObjectWithTag(tagMask).GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
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

        public void Instance(SpriteRenderer spriteRenderer)
        {
            entitySpriteRenderer = spriteRenderer;
        }
    }
}