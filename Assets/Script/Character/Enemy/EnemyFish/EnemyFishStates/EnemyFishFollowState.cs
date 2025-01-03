using UnityEngine;

public class EnemyFishFollowState : IState
{
    public EnemyFishFollowState(EnemyFish enemyFish, float followRadius, float attackRadius) =>
        (_enemyFish, _followRadius, _attackRadius) = (enemyFish, followRadius, attackRadius);

    public StateMachine StateMachine { get; }
    EnemyFish _enemyFish;
    float _followRadius;
    float _attackRadius;


    public void Enter()
    {
        _enemyFish.Animator.SetBool("IsRunning", true);

        _enemyFish.InvokeRepeating(nameof(_enemyFish.UpdatePath), 0, 0.5f);
    }

    public void Execute()
    {
        if (_enemyFish.IsPlayerInRadius(_attackRadius))
        {
            _enemyFish.StateMachine.ChangeState(_enemyFish.AttackState);
        }
        else if (!_enemyFish.IsPlayerInRadius(_followRadius))
        {
            _enemyFish.StateMachine.ChangeState(_enemyFish.IdleState);
        }
    }

    public void FixedExecute()
    {
    }

    public void Exit()
    {
        _enemyFish.Animator.SetBool("IsRunning", false);

        _enemyFish.CancelInvoke(nameof(_enemyFish.UpdatePath));

        _enemyFish._rb.velocity = Vector2.zero;
    }
}

