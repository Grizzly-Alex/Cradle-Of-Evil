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
        currentState?.LogicUpdate();
    }

    private void FixedUpdate()
    {
        currentState?.PhysicsUpdate();       
    }

    private void AnimationTrigger() => currentState.AnimationTrigger();
    private void AnimationFinishTrigger() => currentState.AnimationFinishTrigger();
}