using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _body;

    private BulletParameters _parameters;

    public Action<Bullet> OnDead { get; set; }

    public void Construct(BulletParameters parameters)
    {
        _parameters = parameters;
        _parameters.TargetedEnemy.OnDead += OnEnemyDead;
    }

    private void OnEnemyDead(Enemy enemy)
    {
        OnDead?.Invoke(this);
    }

    private void FixedUpdate()
    {
        var direction = (_parameters.TargetedEnemy.transform.position - transform.position).normalized;
        _body.velocity = direction * _parameters.Speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Bullet might damage another enemy than the one targeted
        var enemy = Enemy.GetFromCollider(collision.collider);
        if (enemy)
        {
            Hit(enemy);
        }
        OnDead?.Invoke(this);
    }

    protected virtual void Hit(Enemy enemy)
    {
        var killed = enemy.Damage(_parameters.Damage);
        if (killed)
        {
            enemy.IncreaseScoreAndGold();
        }
    }
}