using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawning a ring of bullets
public class RingPattern : BulletPattern
{
    //Density of the ring (in bullets/360 degrees, functionally total bullets)
    protected int _density;

    public RingPattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density)
    {
        this._spawnPoint = spawnPoint;
        this._bulletPrefab = bulletPrefab;
        this.direction = direction;
        this._density = density;
    }

    public override void Spawn()
    {
        float spacing = 360 / _density;
        for (int i = 0; i < _density; i++)
        {
            float deltaAngle = i * spacing;
            Object.Instantiate(_bulletPrefab, _spawnPoint.position + (Vector3)offset, Quaternion.Euler(new(0, 0, direction + deltaAngle)));
        }
    }
}
