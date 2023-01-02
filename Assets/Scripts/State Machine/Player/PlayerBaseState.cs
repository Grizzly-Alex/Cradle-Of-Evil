using UnityEngine;


public abstract class PlayerBaseState: IState
{  
    protected bool _isAnimationFinished;
    protected PlayerStateMachine playerSm;
    protected bool _isGrounded;

    
    public PlayerBaseState(PlayerStateMachine playerSm)
    {
        this.playerSm = playerSm;
    }
    
    #region State Machine
    public virtual void Enter()
    {
        DoCheck();
        
        _isAnimationFinished = false; 
    }

    public virtual void DoCheck()
    {
        _isGrounded = playerSm.Core.CollisionSensors.GroundDetect;
    }

    public virtual void LogicUpdate()
    { 
        playerSm.Core.LogicUpdate();   
    }

    public virtual void PhysicsUpdate()
    {     
        DoCheck();      
    }
    
    public virtual void Exit()
    {  
           
    }
    #endregion 

    #region Trigers
    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => _isAnimationFinished = true;
    #endregion

    #region Methods
    protected void SetColliderHeight(float height)
    {
        Vector2 center = playerSm.BodyCollider.offset;
        Vector2 newSize = new(playerSm.BodyCollider.size.x, height); 
        center.y += (height - playerSm.BodyCollider.size.y) / 2;
        playerSm.BodyCollider.size = newSize;
        playerSm.BodyCollider.offset = center;
    }
    #endregion
}