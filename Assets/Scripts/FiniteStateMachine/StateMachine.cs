using Interfaces;


namespace FiniteStateMachine
{
    public sealed class StateMachine
    {
        public IState CurrentState { get; private set; }
        public IState PreviousState { get; private set; }


        public void InitState(IState initState)
        {
            CurrentState = initState;
            CurrentState.Enter();
        }

        public void ChangeState(IState newState)
        {
            PreviousState = CurrentState;
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}