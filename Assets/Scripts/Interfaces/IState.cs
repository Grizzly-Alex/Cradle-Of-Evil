public interface IState: IAnimationTriger
{
    public void Enter();
    public void DoCheck(); 
    public void LogicUpdate();
    public void PhysicsUpdate();
    public void Exit();  
}