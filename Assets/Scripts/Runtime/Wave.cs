using System;
using UnityEngine;

[Serializable]
public class Wave
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private int _count;
    [SerializeField] private float _delay;

    public EnemySpawner EnemySpawner => _enemySpawner;
    public EnemyData EnemyData => _enemyData;
    public int Count => _count;
    public float Delay => _delay;
}