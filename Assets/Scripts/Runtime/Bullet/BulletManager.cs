using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    private static BulletManager _instance;

    public static BulletManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BulletManager();
            }

            return _instance;
        }
    }

    private Dictionary<int, BulletPool> _instanceIdToBulletPoolDic = new Dictionary<int, BulletPool>();
    private readonly Transform _allBulletParent;

    public BulletManager()
    {
        _allBulletParent = new GameObject("Bullets").transform;
        UnityEngine.Object.DontDestroyOnLoad(_allBulletParent.gameObject);
    }

    public Bullet GetNewBullet(Bullet bulletPrefab, Vector3 pos, Quaternion rot)
    {
        var instanceId = bulletPrefab.GetInstanceID();
        if (!_instanceIdToBulletPoolDic.TryGetValue(instanceId, out BulletPool bulletPool))
        {
            var parent = new GameObject($"{bulletPrefab.name}s").transform;
            parent.SetParent(_allBulletParent);
            bulletPool = new BulletPool(bulletPrefab, parent);
            _instanceIdToBulletPoolDic.Add(instanceId, bulletPool);
        }

        return bulletPool.GetNewBullet(pos, rot);
    }
}