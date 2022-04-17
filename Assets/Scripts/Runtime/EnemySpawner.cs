using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Path _path;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private int _amount;

    private Queue<EnemySpawnCommand> _spawnRoutines = new Queue<EnemySpawnCommand>();
    private bool _spawning;

    private void Awake()
    {
        Spawn(_enemyData, _amount);
    }

    public void Spawn(EnemyData enemyData, int amount)
    {
        var command = new EnemySpawnCommand(_gameManager, _path, enemyData, amount, OnSpawnComplete);
        if (_spawning)
        {
            _spawnRoutines.Enqueue(command);
        }
        else
        {
            ExecuteCommand(command);
        }
    }

    private void ExecuteCommand(EnemySpawnCommand enemySpawnCommand)
    { 
        _spawning = true;
        StartCoroutine(enemySpawnCommand.SpawnRoutine());
    }
    
    private void OnSpawnComplete()
    {
        if (_spawnRoutines.Count > 0)
        {
            ExecuteCommand(_spawnRoutines.Dequeue());
        }
        else
        {
            _spawning = false;
        }
    }
}