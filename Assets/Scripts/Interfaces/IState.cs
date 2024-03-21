namespace Interfaces
{
    public interface IState : IAnimationFinishTrigger, IAnimationTrigger
    {
        public void Enter();
        public void LogicUpdate();
        public void PhysicsUpdate();
        public void Exit();
    }
}