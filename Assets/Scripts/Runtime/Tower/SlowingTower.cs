
using UnityEngine;

public class SlowingTower : BaseTower
{
    [SerializeField] private SlowingTowerStats _slowingTowerStats;
    
    protected override TowerStats GetTowerStats()
    {
        return _slowingTowerStats;
    }

    protected override void OnBulletHit(Enemy enemy, Bullet bullet)
    {
        bullet.OnHit -= OnBulletHit;
        var diedAfterDamage = enemy.Damage(GetTowerStats().Damage);
        if (diedAfterDamage)
        {
            enemy.IncreaseScoreAndGold();
        }
        else
        {
            enemy.ApplySlow(_slowingTowerStats.SlowingRate, _slowingTowerStats.SlowDuration);
        }
    }
}