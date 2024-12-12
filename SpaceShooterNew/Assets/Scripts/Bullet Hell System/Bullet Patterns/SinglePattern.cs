using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawning a single bullet
[PatternType(PatternType.Single)]
public class SinglePattern : BulletPattern
{
    public SinglePattern()
    {
        spawnPoint = null;
        bulletPrefab = null;
        direction = 0f;
    }

    public SinglePattern(Transform spawnPoint, GameObject bulletPrefab, float direction)
    {
        this.spawnPoint = spawnPoint;
        this.bulletPrefab = bulletPrefab;
        this.direction = direction;
    }

    public override void Spawn()
    {
        Object.Instantiate(bulletPrefab, spawnPoint.position + (Vector3)offset, Quaternion.Euler(new(0, 0, direction)));
    }
}
