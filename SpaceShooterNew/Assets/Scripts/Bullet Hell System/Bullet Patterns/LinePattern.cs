using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePattern : BulletPattern
{
    protected int _density;
    protected float _length;

    public LinePattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float length)
    {
        _spawnPoint = spawnPoint;
        _bulletPrefab = bulletPrefab;
        _direction = direction;
        _density = density;
        _length = length;
    }

    public override void Spawn()
    {
        float spacing = 1 / (float)_density * _length;
        for (int i = 0; i < _density; i++)
        {
            float distance = i * spacing - _length / 2;
            Vector3 deltaPos = new Vector3(-Mathf.Sin((_direction - 90) * Mathf.Deg2Rad), -Mathf.Cos((_direction - 90) * Mathf.Deg2Rad)) * distance;
            Object.Instantiate(_bulletPrefab, _spawnPoint.position + deltaPos, Quaternion.Euler(new(0, 0, _direction)));
        }
    }
}
