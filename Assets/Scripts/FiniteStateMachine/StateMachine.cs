using Interfaces;


namespace FiniteStateMachine
{
    public sealed class StateMachine
    {
        public IState CurrentState { get; private set; }

        public void InitState(IState initState)
        {
            CurrentState = initState;
            CurrentState.Enter();
        }

        public void ChangeState(IState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}