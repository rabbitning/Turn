using UnityEngine;

public class EnemyPenguinFollowState : IState
{
    private EnemyPenguin _enemyPenguin;
    float _followRadius;

    public EnemyPenguinFollowState(EnemyPenguin enemyPenguin, float followRadius) =>
        (_enemyPenguin, _followRadius) = (enemyPenguin, followRadius);

    public void Enter()
    {
        _enemyPenguin.InvokeRepeating(nameof(_enemyPenguin.UpdatePath), 0, 0.5f);
    }

    public void Execute()
    {
        if (!_enemyPenguin.IsPlayerInRadius(_followRadius))
        {
            _enemyPenguin.StateMachine.ChangeState(_enemyPenguin.IdleState);
        }
    }

    public void FixedExecute()
    {
    }

    public void Exit()
    {
        _enemyPenguin.CancelInvoke(nameof(_enemyPenguin.UpdatePath));

        _enemyPenguin._rb.velocity = Vector2.zero;
    }
}