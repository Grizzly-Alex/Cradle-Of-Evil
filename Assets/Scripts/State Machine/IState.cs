public interface IState 
{
    public void Enter();
    public void LogicUpdate(float deltaTime);
    public void Exit();
}
