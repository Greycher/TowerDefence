using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = nameof(TowerStats), menuName = GlobalConst.DataMenuName + "/" + nameof(TowerStats))]
public class TowerStats : ScriptableObject
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _damage;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _bulletSpeed;

    public Bullet BulletPrefab => _bulletPrefab;
    public float Damage => _damage;
    public float FireRate => _fireRate;
    public float BulletSpeed => _bulletSpeed;
}