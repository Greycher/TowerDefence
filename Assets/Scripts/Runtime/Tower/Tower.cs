using System.Collections.Generic;
using UnityEngine;

public class Tower : BaseTower
{
    [SerializeField] private TowerStats _towerStats;
    
    protected override TowerStats GetTowerStats()
    {
        return _towerStats;
    }
}