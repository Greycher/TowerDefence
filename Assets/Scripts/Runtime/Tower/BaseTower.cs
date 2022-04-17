
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [SerializeField] protected Transform _bulletSpawnPosTransformation;
    
    private LinkedList<Enemy> _enemiesInRange = new LinkedList<Enemy>();
    private Enemy _targetedEnemy;
    private float _shootCoolDown;
    private float _lastShotTime = float.MinValue;

    protected abstract TowerStats GetTowerStats();

    private void Awake()
    {
        _shootCoolDown = 1 / GetTowerStats().FireRate;
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
    
    private void Shoot()
    {
        var pos = _bulletSpawnPosTransformation.position;
        var bullet = BulletManager.Instance.GetNewBullet(GetTowerStats().BulletPrefab, pos, Quaternion.identity);
        bullet.OnHit += OnBulletHit;
        bullet.Construct(new BulletParameters(_targetedEnemy, GetTowerStats().Damage, GetTowerStats().BulletSpeed));
    }

    protected virtual void OnBulletHit(Enemy enemy, Bullet bullet)
    {
        bullet.OnHit -= OnBulletHit;
        var diedAfterDamage = enemy.Damage(GetTowerStats().Damage);
        if (diedAfterDamage)
        {
            enemy.IncreaseScoreAndGold();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        var enemy = Enemy.GetFromCollider(collider);
        if (enemy.IsFlyingUnit && !GetTowerStats().CanTargetFlyingUnits)
        {
            return;
        }
        
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
        if (enemy.IsFlyingUnit && !GetTowerStats().CanTargetFlyingUnits)
        {
            return;
        }
        
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