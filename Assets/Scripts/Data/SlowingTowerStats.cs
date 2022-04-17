using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = nameof(SlowingTowerStats), menuName = GlobalConst.DataMenuName + "/" + nameof(SlowingTowerStats))]
public class SlowingTowerStats : TowerStats
{
    [SerializeField] private float _slowingRate = 0.2f;
    [SerializeField] private float _slowDuration = 1f;

    public float SlowingRate => _slowingRate;
    public float SlowDuration => _slowDuration;
}