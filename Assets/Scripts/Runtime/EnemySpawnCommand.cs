
using System;
using System.Collections;
using UnityEngine;

public class EnemySpawnCommand
{
    private readonly EnemyData _enemyData;
    private int _amount;
    private readonly Action _onComplete;
    private readonly GameManager _gameManager;
    private readonly Path _path;

    public EnemySpawnCommand(GameManager gameManager, Path path, EnemyData enemyData, int amount, Action onComplete)
    {
        _gameManager = gameManager;
        _path = path;
        _enemyData = enemyData;
        _amount = amount;
        _onComplete = onComplete;
    }

    public IEnumerator SpawnRoutine()
    {
        var pos = _path.GetPosition(0);
        var rot = Quaternion.LookRotation(_path.GetPosition(0));
        var waitTime = 1 / _enemyData.SpawnFrequency;

        while (_amount > 0)
        {
            _amount--;
            var enemy = UnityEngine.Object.Instantiate(_enemyData.EnemyPrefab, pos, rot);
            enemy.Construct(_gameManager, _path);
            yield return new WaitForSeconds(waitTime);
        }

        _onComplete?.Invoke();
    }
}