using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerStats _stats;
    [SerializeField] private Transform _bulletSpawnPosTransformation;
    
    private LinkedList<Enemy> _enemiesInRange = new LinkedList<Enemy>();
    private Enemy _targetedEnemy;
    private float _shootCoolDown;
    private float _lastShotTime = float.MinValue;
    
    private void Awake()
    {
        _shootCoolDown = 1 / _stats.FireRate;
    }

    private void Update()
    {
        if (_targetedEnemy)
        {
            if (Time.time - _lastShotTime > _shootCoolDown)
            {
                _lastShotTime = Time.time;
                Shoot();
            }
        }
    }

    protected virtual void Shoot()
    {
        var pos = _bulletSpawnPosTransformation.position;
        var bullet = BulletManager.Instance.GetNewBullet(_stats.BulletPrefab, pos, Quaternion.identity);
        bullet.Construct(new BulletParameters(_targetedEnemy, _stats.Damage, _stats.BulletSpeed));
    }

    private void OnTriggerEnter(Collider collider)
    {
        var enemy = Enemy.GetFromCollider(collider);
        if (enemy)
        {
            enemy.OnDead += OnEnemyDead;
            _enemiesInRange.AddLast(enemy);
            if (!_targetedEnemy)
            {
                UpdateTarget();
            }
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        var enemy = Enemy.GetFromCollider(collider);
        if (enemy)
        {
            enemy.OnDead -= OnEnemyDead;
            _enemiesInRange.Remove(enemy);
            if (enemy == _targetedEnemy)
            {
                UpdateTarget();
            }
        }
    }

    private void UpdateTarget()
    {
        if (_enemiesInRange.Count > 0)
        {
            _targetedEnemy = _enemiesInRange.First.Value;
        }
        else
        {
            _targetedEnemy = null;
        }
    }

    private void OnEnemyDead(Enemy enemy)
    {
        enemy.OnDead -= OnEnemyDead;
        _enemiesInRange.Remove(enemy);
        if (enemy == _targetedEnemy)
        {
            UpdateTarget();
        }
    }
}