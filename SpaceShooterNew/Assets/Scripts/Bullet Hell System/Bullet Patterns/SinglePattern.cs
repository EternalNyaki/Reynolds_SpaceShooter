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
        offset = Vector2.zero;
        bulletPrefab = null;
        direction = 0f;
    }

    public SinglePattern(Transform spawnPoint, Vector2 offset, GameObject bulletPrefab, float direction)
    {
        this.spawnPoint = spawnPoint;
        this.offset = offset;
        this.bulletPrefab = bulletPrefab;
        this.direction = direction;
    }

    public override void Spawn()
    {
        Object.Instantiate(bulletPrefab, spawnPoint.position + (Vector3)offset, Quaternion.Euler(new(0, 0, direction)));
    }
}
