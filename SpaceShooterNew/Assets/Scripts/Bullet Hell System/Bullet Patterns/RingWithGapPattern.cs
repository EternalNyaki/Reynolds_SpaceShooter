using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingWithGapPattern : RingPattern
{
    protected float _gapSize;

    public RingWithGapPattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float gapSize) : base(spawnPoint, bulletPrefab, direction, density)
    {
        _gapSize = gapSize;
    }

    public override void Spawn()
    {
        float spacing = 360 / _density;
        for (int i = 0; i < _density; i++)
        {
            float deltaAngle = i * spacing;
            if (deltaAngle > spacing && 360 - deltaAngle > spacing)
            {
                Object.Instantiate(_bulletPrefab, _spawnPoint.position, Quaternion.Euler(new(0, 0, _direction + deltaAngle)));
            }
        }
    }
}
