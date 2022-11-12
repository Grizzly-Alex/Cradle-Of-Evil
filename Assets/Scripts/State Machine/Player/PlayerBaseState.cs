using UnityEngine;
using System;

public abstract class PlayerBaseState : IState
{
    protected bool isAnimationFinished;
    protected PlayerStateMachine stateMachine;
    protected InputReader input;
    protected Core core;
    protected Animator animator; 
    protected MaterialsData materialsData;
    protected PlayerData playerData;
    protected bool isGrounded;

    
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerData = stateMachine.PlayerData;
        materialsData = stateMachine.MaterialsData;
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

    public virtual void LogicUpdate()
    {
        core.Movement.LogicUpdate();
    }

    public virtual void PhysicsUpdate()
    {     
        DoCheck();      
    }
    
    public virtual void Exit()
    {

    }

    public virtual void DoCheck()
    {
        isGrounded = core.CollisionSenses.DetectingGround();
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

    protected void SetPhysMaterial(PhysicsMaterial2D newMaterial)
    {
        if(stateMachine.Rigidbody.sharedMaterial.name.Equals
        (newMaterial.name, StringComparison.Ordinal)) return;
       
        stateMachine.Rigidbody.sharedMaterial = newMaterial;
    }  

    protected void SwitchPhysMaterial(int InputX)
    {
        switch (input.NormInputX)
        {
            case 0: SetPhysMaterial(materialsData.Friction); break;
            default: SetPhysMaterial(materialsData.NoFriction); break;
        }
    } 
    #endregion    
}