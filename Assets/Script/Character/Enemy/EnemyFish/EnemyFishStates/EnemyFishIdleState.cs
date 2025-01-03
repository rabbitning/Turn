using UnityEngine;

public class EnemyFishIdleState : IState
{
    public EnemyFishIdleState(EnemyFish enemyFish, float followRadius) =>
        (_enemyFish, _followRadius) = (enemyFish, followRadius);

    EnemyFish _enemyFish;
    float _followRadius;

    public void Enter()
    {
    }

    public void Execute()
    {
        if (_enemyFish.IsPlayerInRadius(_followRadius))
        {
            _enemyFish.StateMachine.ChangeState(_enemyFish.FollowState);
        }
    }

    public void FixedExecute()
    {
    }

    public void Exit()
    {
    }
}

