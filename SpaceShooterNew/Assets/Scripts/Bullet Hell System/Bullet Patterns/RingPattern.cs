using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawning a ring of bullets
[PatternType(PatternType.Ring)]
public class RingPattern : BulletPattern
{
    //Density of the ring (in bullets/360 degrees, functionally total bullets)
    public int density;

    public RingPattern()
    {
        spawnPoint = null;
        offset = Vector2.zero;
        bulletPrefab = null;
        direction = 0f;
        density = 0;
    }

    public RingPattern(Transform spawnPoint, Vector2 offset, GameObject bulletPrefab, float direction, int density)
    {
        this.spawnPoint = spawnPoint;
        this.offset = offset;
        this.bulletPrefab = bulletPrefab;
        this.direction = direction;
        this.density = density;
    }

    public override void Spawn()
    {
        float spacing = 360 / density;
        for (int i = 0; i < density; i++)
        {
            float deltaAngle = i * spacing;
            Object.Instantiate(bulletPrefab, spawnPoint.position + (Vector3)offset, Quaternion.Euler(new(0, 0, direction + deltaAngle)));
        }
    }
}
