using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Path _path;

    private Queue<EnemySpawnCommand> _spawnRoutines = new Queue<EnemySpawnCommand>();
    private bool _spawning;
    
    public void Spawn(EnemyData enemyData, int count)
    {
        var command = new EnemySpawnCommand(_gameManager, _path, enemyData, count, OnSpawnComplete);
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