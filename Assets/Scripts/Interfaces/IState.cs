namespace Interfaces
{
    public interface IState : IAnimationFinishTrigger, IAnimationTrigger
    {
        public void Enter();
        public void Update();
        public void Exit();
    }
}