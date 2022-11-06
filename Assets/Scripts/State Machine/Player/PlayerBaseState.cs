using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class PlayerBaseState : IState
{
    protected bool isAnimationFinished = false;
    protected PlayerStateMachine stateMachine;
    protected InputReader input;
    protected Core core;
    protected Animator animator; 
    protected MaterialsData materialsData;
    protected PlayerData playerData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerData = stateMachine.PlayerData;
        materialsData = stateMachine.MaterialsData;
        input = stateMachine.InputReader;
        core = stateMachine.Core;
        animator = stateMachine.Animator;
    }
    
    public abstract void Enter();
    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();
    public abstract void Exit();
    public virtual void AnimationTriger() { }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    
    protected void SetColliderHeight(float height)
    {
        Vector2 center = stateMachine.BodyCollider.offset;
        Vector2 newSize = new(stateMachine.BodyCollider.size.x, height); 
        center.y += (height - stateMachine.BodyCollider.size.y) / 2;
        stateMachine.BodyCollider.size = newSize;
        stateMachine.BodyCollider.offset = center;
    }

    protected void SetPhysicsMaterial(PhysicsMaterial2D newMaterial)
    {
        if(stateMachine.Rigidbody.sharedMaterial.name.Equals
        (newMaterial.name, StringComparison.Ordinal)) return;
       
        stateMachine.Rigidbody.sharedMaterial = newMaterial;
    }   
}