using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected InputReader input;
    protected Core core;
    protected int currentHashAnim;
    protected PlayerData data;


    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        data = stateMachine.PlayerData;
        input = stateMachine.InputReader;
        core = stateMachine.Core;
    }
    
    public abstract void Enter();
    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();
    public abstract void Exit();
}