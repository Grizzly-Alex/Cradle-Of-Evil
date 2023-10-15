namespace Interfaces
{
    public interface IState : ITriggerAnimation
    {
        public void Enter();
        public void Update();
        public void Exit();
    }
}