public interface IState
{
    public abstract void Enter();
    public abstract void Execute();
    public abstract void FixedExecute();
    public abstract void Exit();
}