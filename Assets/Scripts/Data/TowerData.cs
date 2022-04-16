using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = nameof(TowerData), menuName = GlobalConst.DataMenuName + "/" + nameof(TowerData))]
public class TowerData : ScriptableObject
{
    public Tower TowerPrefab;
    public TowerGridIndicator GridIndicator;
    public int Cost;
}