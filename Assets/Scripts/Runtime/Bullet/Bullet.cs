using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _body;

    private BulletParameters _parameters;

    public Action<Enemy, Bullet> OnHit { get; set; }
    public Action<Bullet> OnDead { get; set; }

    public void Construct(BulletParameters parameters)
    {
        _parameters = parameters;
    }
    
    private void FixedUpdate()
    {
        if (_parameters.TargetedEnemy)
        {
            var direction = (_parameters.TargetedEnemy.GetPosition() - transform.position).normalized;
            _body.velocity = direction * _parameters.Speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = Enemy.GetFromCollider(collision.collider);
        if (enemy)
        {
            OnHit?.Invoke(enemy, this);
        }
        OnDead?.Invoke(this);
    }
}