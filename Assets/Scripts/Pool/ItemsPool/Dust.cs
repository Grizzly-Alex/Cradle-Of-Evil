using Interfaces;
using UnityEngine;

namespace Pool.ItemsPool
{
    public sealed class Dust : PooledObject, ITriggerAnimation
    {
        private Animator animator;

        private readonly int afterMove = Animator.StringToHash("AfterMoveDust");
        private readonly int jumpFromGround = Animator.StringToHash("JumpFromGroundDust");
        private readonly int jumpFromWall = Animator.StringToHash("JumpFromWallDust");
        private readonly int landingOnGround = Animator.StringToHash("LandingOnGroundDust");
        private readonly int tiny = Animator.StringToHash("TinyDust");
        private readonly int startSlide = Animator.StringToHash("StartSlideDust");
        private readonly int big = Animator.StringToHash("BigDust");
        private readonly int hardLandingOnGround = Animator.StringToHash("HardLandingOnGroundDust");
        private readonly int sliding = Animator.StringToHash("SlidingDust");
        private readonly int dash = Animator.StringToHash("DashDust");
        private readonly int landingOnWall = Animator.StringToHash("LandingOnWallDust");


        [SerializeField]
        private int currentHashAnimation;
        private bool isAnimFinished;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            isAnimFinished = false;

            if(currentHashAnimation != default)
            {
                animator.Play(currentHashAnimation);
            }
        }

        private void Update()
        {
            if (isAnimFinished)
            {
                PoolManger.Instance.ReturnToPool(gameObject);
            }
        }

        public override void Get(GameObject obj)
        {
            obj.SetActive(false);
        }

        public override GameObject Create(Transform container)
        {
            gameObject.SetActive(false);
            return base.Create(container);
        }

        public void Initialize(DustType dust) => currentHashAnimation = GetAnimationHash(dust);

        public void AnimationFinishTrigger()      
             => isAnimFinished = true;
        
        public void AnimationTrigger(){ }

        private int GetAnimationHash(DustType dust)
            => dust switch
            {
                DustType.AfterMove => afterMove,
                DustType.JumpFromGround => jumpFromGround,
                DustType.JumpFromWall => jumpFromWall,
                DustType.LandingOnGround => landingOnGround,
                DustType.Tiny => tiny,
                DustType.StartSlide => startSlide,
                DustType.Big => big,
                DustType.HardLandingOnGround => hardLandingOnGround,
                DustType.Sliding => sliding,
                DustType.Dash => dash,
                DustType.LandingOnWall => landingOnWall,
                _ => default
            };
    }

    public enum DustType
    {
        AfterMove = 1,
        JumpFromGround = 2,
        JumpFromWall = 3,
        LandingOnGround = 4,
        Tiny = 5,
        StartSlide = 6,
        Big = 7,
        HardLandingOnGround = 8,
        Sliding = 9,
        Dash = 10,
        LandingOnWall = 11,
    }
}
