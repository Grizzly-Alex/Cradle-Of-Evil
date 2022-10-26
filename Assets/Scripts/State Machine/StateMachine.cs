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
        currentState?.LogicUpdate(Time.deltaTime);
    }
}