using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePattern : BulletPattern
{
    public SinglePattern(Transform spawnPoint, GameObject bulletPrefab, float direction)
    {
        _spawnPoint = spawnPoint;
        _bulletPrefab = bulletPrefab;
        _direction = direction;
    }

    public override void Spawn()
    {
        Object.Instantiate(_bulletPrefab, _spawnPoint.position, Quaternion.Euler(new(0, 0, _direction)));
    }
}
