using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = nameof(EnemyStats), menuName = GlobalConst.DataMenuName + "/" + nameof(EnemyStats))]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float _speed;
    [SerializeField] private float _health;
    [SerializeField] private int _prizeCoinAmount;
    [SerializeField] private int _prizeScoreAmount;
    [SerializeField] private bool _isFlyingUnit;

    public float Speed => _speed;
    public float Health => _health;
    public int PrizeCoinAmount => _prizeCoinAmount;
    public int PrizeScoreAmount => _prizeScoreAmount;
    public bool IsFlyingUnit => _isFlyingUnit;
}