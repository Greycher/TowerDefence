using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = nameof(EnemyData), menuName = GlobalConst.DataMenuName + "/" + nameof(EnemyData))]
public class EnemyData : ScriptableObject
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnFrequency;

    public Enemy EnemyPrefab => _enemyPrefab;
    public float SpawnFrequency => _spawnFrequency;
}