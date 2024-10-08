using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPattern : BulletPattern
{
    protected int _density;

    public RingPattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density)
    {
        _spawnPoint = spawnPoint;
        _bulletPrefab = bulletPrefab;
        _direction = direction;
        _density = density;
    }

    public override void Spawn()
    {
        float spacing = 360 / _density;
        for (int i = 0; i < _density; i++)
        {
            float deltaAngle = i * spacing;
            Object.Instantiate(_bulletPrefab, _spawnPoint.position, Quaternion.Euler(new(0, 0, _direction + deltaAngle)));
        }
    }
}
