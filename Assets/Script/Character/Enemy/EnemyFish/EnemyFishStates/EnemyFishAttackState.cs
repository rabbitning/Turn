using UnityEngine;

public class EnemyFishAttackState : IState
{
    public EnemyFishAttackState(EnemyFish enemyFish, float attackRadius, float attackCooldown) =>
        (_enemyFish, _attackRadius, _attackCooldown) = (enemyFish, attackRadius, attackCooldown);

    EnemyFish _enemyFish;
    float _attackRadius;
    float _attackCooldown;
    float _attackTimer = 0;

    public void Enter()
    {
    }

    public void Execute()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
        {
            _enemyFish.Animator.SetTrigger("Attack");

            // _enemyFish.Attack();
            _attackTimer = _attackCooldown;
        }

        if (!_enemyFish.IsPlayerInRadius(_attackRadius))
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