using UnityEngine;


public abstract class PlayerBaseState : IState
{  
    protected bool isAnimationFinished;
    protected PlayerStateMachine stateMachine;
    protected InputReader input;
    protected Core core;
    protected Animator animator; 
    protected PlayerData playerData;
    protected bool isGrounded;

    
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerData = stateMachine.PlayerData;
        input = stateMachine.InputReader;
        core = stateMachine.Core;
        animator = stateMachine.Animator;
    }
    
    #region State Machine
    public virtual void Enter()
    {
        DoCheck();
        isAnimationFinished = false;      
    }

    public virtual void DoCheck()
    {
        isGrounded = core.CollisionSensors.GroundDetect;
        Debug.Log(core.CollisionSensors.GrabWallDetect);
    }

    public virtual void LogicUpdate()
    { 
        core.LogicUpdate();            
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
    public virtual void AnimationTriger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
    #endregion

    #region Methods
    protected void SetColliderHeight(float height)
    {
        Vector2 center = stateMachine.BodyCollider.offset;
        Vector2 newSize = new(stateMachine.BodyCollider.size.x, height); 
        center.y += (height - stateMachine.BodyCollider.size.y) / 2;
        stateMachine.BodyCollider.size = newSize;
        stateMachine.BodyCollider.offset = center;
    }
    #endregion
}