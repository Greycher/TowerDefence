using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = nameof(TowerData), menuName = GlobalConst.DataMenuName + "/" + nameof(TowerData))]
public class TowerData : ScriptableObject
{
    [SerializeField] private BaseTower _towerPrefab;
    [SerializeField] private TowerGridIndicator _gridIndicator;
    [SerializeField] private int _coinCostAmount;

    public BaseTower TowerPrefab => _towerPrefab;
    public TowerGridIndicator GridIndicator => _gridIndicator;
    public int CoinCostAmount => _coinCostAmount;
}