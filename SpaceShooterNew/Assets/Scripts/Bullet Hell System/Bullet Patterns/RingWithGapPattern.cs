using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawning a ring a bullets with a gap
public class RingWithGapPattern : RingPattern
{
    //Size of the gap (in degrees)
    protected float _gapSize;

    public RingWithGapPattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float gapSize) : base(spawnPoint, bulletPrefab, direction, density)
    {
        this._gapSize = gapSize;
    }

    public override void Spawn()
    {
        float spacing = 360 / _density;
        for (int i = 0; i < _density; i++)
        {
            float deltaAngle = i * spacing;
            if (deltaAngle > _gapSize / 2 && 360 - deltaAngle > _gapSize / 2)
            {
                Object.Instantiate(_bulletPrefab, _spawnPoint.position + (Vector3)offset, Quaternion.Euler(new(0, 0, direction + deltaAngle)));
            }
        }
    }
}
