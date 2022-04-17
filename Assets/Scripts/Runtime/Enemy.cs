using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats _stats;
    
    private Path _path;
    private GameManager _gameManager;
    private float _currentWayPoint;
    private float _speed;
    private float _health;

    public Action<Enemy> OnDead { get; set; }

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
        if (_health <= 0)
        {
            return false;
        }
        
        _health -= damage;
        if (_health <= 0)
        {
            Kill();
            return true;
        }

        return false;
    }

    public void Kill()
    {
        OnDead?.Invoke(this);
        Destroy(gameObject);
    }

    public void IncreaseScoreAndGold()
    {
        _gameManager.AddScore(_stats.PrizeScoreAmount);
        _gameManager.AddGold(_stats.PrizeCoinAmount);
    }
}