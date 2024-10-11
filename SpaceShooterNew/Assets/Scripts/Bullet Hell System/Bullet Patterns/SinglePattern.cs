using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawning a single bullet
public class SinglePattern : BulletPattern
{
    public SinglePattern(Transform spawnPoint, GameObject bulletPrefab, float direction)
    {
        this._spawnPoint = spawnPoint;
        this._bulletPrefab = bulletPrefab;
        this.direction = direction;
    }

    public override void Spawn()
    {
        Object.Instantiate(_bulletPrefab, _spawnPoint.position + (Vector3)offset, Quaternion.Euler(new(0, 0, direction)));
    }
}
