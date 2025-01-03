public class EnemyPenguinIdleState : IState
{
    private EnemyPenguin _enemyPenguin;
    float _followRadius;

    public EnemyPenguinIdleState(EnemyPenguin enemyPenguin, float followRadius) =>
        (_enemyPenguin, _followRadius) = (enemyPenguin, followRadius);

    public void Enter()
    {
    }

    public void Execute()
    {
        if (_enemyPenguin.IsPlayerInRadius(_followRadius))
        {
            _enemyPenguin.StateMachine.ChangeState(_enemyPenguin.FollowState);
        }
    }

    public void FixedExecute()
    {
    }

    public void Exit()
    {
    }
}