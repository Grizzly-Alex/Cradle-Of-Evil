using Interfaces;
using UnityEngine;


namespace Pool.ItemsPool
{
    public sealed class Dust : PooledObject, IAnimationFinishTrigger
    {
        [SerializeField]
        private Transform updatedTransform;
        [SerializeField]
        private Vector2 offsetPosition;
        private Vector2 workingVector;

        [SerializeField]
        private int currentHashAnimation;
        private Animator animator;

        #region Hash Animations
        private readonly int afterMove = Animator.StringToHash("AfterMoveDust");
        private readonly int jumpFromGround = Animator.StringToHash("JumpFromGroundDust");
        private readonly int jumpFromWall = Animator.StringToHash("JumpFromWallDust");
        private readonly int landing = Animator.StringToHash("LandingDust");
        private readonly int hardLanding = Animator.StringToHash("HardLandingDust");
        private readonly int tiny = Animator.StringToHash("TinyDust");
        private readonly int startSlide = Animator.StringToHash("StartSlideDust");
        private readonly int brake = Animator.StringToHash("BrakeDust");
        private readonly int sliding = Animator.StringToHash("SlidingDust");
        private readonly int dash = Animator.StringToHash("DashDust");
        #endregion

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if(currentHashAnimation != default)
            {
                animator.Play(currentHashAnimation);
            }
        }

        private void Update()
        {
            if (updatedTransform == null) return;
            
            workingVector.Set(
                updatedTransform.rotation.y < 0 
                    ? updatedTransform.position.x - offsetPosition.x 
                    : updatedTransform.position.x + offsetPosition.x,
                updatedTransform.position.y + offsetPosition.y);

            gameObject.transform.SetPositionAndRotation(workingVector, updatedTransform.rotation);           
        }

        private void OnDisable()
        {
            updatedTransform = null;   
            currentHashAnimation = default;
            offsetPosition = default;
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

        public void Initialize(DustType dust) 
            => currentHashAnimation = GetAnimationHash(dust);

        public void Initialize(DustType dust, Transform transform, Vector2 offset)
        {
            currentHashAnimation = GetAnimationHash(dust);
            updatedTransform = transform;
            offsetPosition = offset;
        }
       
        private int GetAnimationHash(DustType dust)
            => dust switch
            {
                DustType.JumpFromGround => jumpFromGround,
                DustType.JumpFromWall => jumpFromWall,
                DustType.HardLanding => hardLanding,
                DustType.StartSlide => startSlide,
                DustType.AfterMove => afterMove,
                DustType.Sliding => sliding,
                DustType.Landing => landing,
                DustType.Brake => brake,
                DustType.Tiny => tiny,
                DustType.Dash => dash,
                _ => default
            };

        public void AnimationFinishTrigger()
             => ReturnToPool();
    }

    public enum DustType : byte
    {
        AfterMove = 1,
        JumpFromGround = 2,
        JumpFromWall = 3,
        Landing = 4,
        Tiny = 5,
        StartSlide = 6,
        Brake = 7,
        HardLanding = 8,
        Sliding = 9,
        Dash = 10,
    }
}
