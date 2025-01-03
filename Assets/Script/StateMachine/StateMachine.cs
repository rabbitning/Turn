public class StateMachine
{
    IState _currentState;

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update() => _currentState?.Execute();

    public void FixedUpdate() => _currentState?.FixedExecute();
}