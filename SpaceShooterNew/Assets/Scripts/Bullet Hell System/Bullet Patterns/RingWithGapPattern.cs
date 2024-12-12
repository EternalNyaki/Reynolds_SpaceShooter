using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawning a ring a bullets with a gap
[PatternType(PatternType.RingWithGap)]
public class RingWithGapPattern : RingPattern
{
    //Size of the gap (in degrees)
    public float gapSize;

    public RingWithGapPattern() : base()
    {
        gapSize = 0f;
    }

    public RingWithGapPattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float gapSize) : base(spawnPoint, bulletPrefab, direction, density)
    {
        this.gapSize = gapSize;
    }

    public override void Spawn()
    {
        float spacing = 360 / density;
        for (int i = 0; i < density; i++)
        {
            float deltaAngle = i * spacing;
            if (deltaAngle > gapSize / 2 && 360 - deltaAngle > gapSize / 2)
            {
                Object.Instantiate(bulletPrefab, spawnPoint.position + (Vector3)offset, Quaternion.Euler(new(0, 0, direction + deltaAngle)));
            }
        }
    }
}
