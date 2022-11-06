using UnityEngine;

public class StateMachine: MonoBehaviour
{
    private IState currentState;

    public void SwitchState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void InitState(IState initState)
    {
        currentState = initState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState?.FrameUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.PhysicsUpdate();       
    }

    private void AnimationTriger() => currentState.AnimationTriger();
    private void AnimationFinishTrigger() => currentState.AnimationFinishTrigger();
}