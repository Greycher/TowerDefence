using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats _stats;
    [SerializeField] private Transform _visual;
    
    private Path _path;
    private GameManager _gameManager;
    private float _currentWayPoint;
    private float _speed;
    private float _health;
    private Coroutine _slowingRoutine;
    private bool _isDead;

    public Action<Enemy> OnDead { get; set; }
    public bool IsFlyingUnit => _stats.IsFlyingUnit;

    public void Construct(GameManager gameManager, Path path)
    {
        _gameManager = gameManager;
        _path = path;
    }

    private void Awake()
    {
        _speed = _stats.Speed;
        _health = _stats.Health;
    }

    private void Update()
    {
        Move();
    }
    
    private void Move()
    {
        var travelDistance = _speed * Time.deltaTime;
        transform.position = _path.CalculateNextPosition(ref _currentWayPoint, travelDistance);
        transform.rotation = Quaternion.LookRotation(_path.GetNormal(_currentWayPoint));
    }

    public static Enemy GetFromCollider(Collider collider)
    {
        return collider.transform.parent.GetComponent<Enemy>();
    }

    public bool Damage(float damage)
    {
        bool diedAfterDamage = false;
        if (_health <= 0)
        {
            return diedAfterDamage;
        }
        
        _health -= damage;
        if (_health <= 0)
        {
            diedAfterDamage = true;
            Kill();
        }
        
        return diedAfterDamage;
    }

    public void Kill()
    {
        if (!_isDead)
        {
            _isDead = true;
            _gameManager.NotifyEnemyDead();
            OnDead?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void IncreaseScoreAndGold()
    {
        _gameManager.AddScore(_stats.PrizeScoreAmount);
        _gameManager.AddGold(_stats.PrizeCoinAmount);
    }

    public void ApplySlow(float slowingRate, float slowDuration)
    {
        if (_slowingRoutine != null)
        {
            StopCoroutine(_slowingRoutine);
        }
        _slowingRoutine = StartCoroutine(SlowingRoutine(slowingRate, slowDuration));
    }

    private IEnumerator SlowingRoutine(float rate, float duration)
    {
        _speed = _stats.Speed * (1 - rate);
        yield return new WaitForSeconds(duration);
        _speed = _stats.Speed;
    }

    public Vector3 GetPosition()
    {
        return _visual.transform.position;
    }

    private void OnDestroy()
    {
        if (!_isDead)
        {
            _isDead = true;
            OnDead?.Invoke(this);
        }
    }
}