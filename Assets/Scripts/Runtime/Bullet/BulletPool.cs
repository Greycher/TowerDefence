using System.Collections.Generic;
using UnityEngine;

public class BulletPool 
{
    private Stack<Bullet> _bulletStack = new Stack<Bullet>();
    private readonly Bullet _bulletPrefab;
    private readonly Transform _bulletParent;

    public BulletPool(Bullet bulletPrefab, Transform bulletParent)
    {
        _bulletPrefab = bulletPrefab;
        _bulletParent = bulletParent;
    }

    public Bullet GetNewBullet(Vector3 pos, Quaternion rot)
    {
        Bullet bullet;
        if (_bulletStack.Count > 0)
        {
            bullet = _bulletStack.Pop();
            var tr = bullet.transform;
            tr.position = pos;
            tr.rotation = rot;
            bullet.gameObject.SetActive(true);
        }
        else
        {
            bullet = CreateNewInstance(pos, rot);
        }

        return bullet;
    }
    
    private Bullet CreateNewInstance(Vector3 pos, Quaternion rot)
    {
        var bullet = UnityEngine.Object.Instantiate(_bulletPrefab, pos, rot, _bulletParent);
        bullet.OnDead += PushToPool;
        return bullet;
    }

    private void PushToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _bulletStack.Push(bullet);
    }
}