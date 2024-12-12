using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawn a line of bullets with a gap
[PatternType(PatternType.LineWithGap)]
public class LineWithGapPattern : LinePattern
{
    //Size of the gap (in units)
    public float gapSize;

    public LineWithGapPattern() : base()
    {
        offset.x = 0f;
        gapSize = 0f;
    }

    public LineWithGapPattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float length, float gapPosition, float gapSize) : base(spawnPoint, bulletPrefab, direction, density, length)
    {
        //Gap position is stored in the offset for more intuitive interaction with position-altering events
        this.offset.x = gapPosition;
        this.gapSize = gapSize;
    }

    public override void Spawn()
    {
        float spacing = 1 / (float)density * length;
        offset.x = Mathf.Clamp(offset.x, -length / 2 + gapSize / 2, length / 2 - gapSize / 2);
        for (int i = 0; i < density; i++)
        {
            float distance = i * spacing - length / 2;
            if (distance < offset.x - gapSize / 2 || distance > offset.x + gapSize / 2)
            {
                Vector3 deltaPos = new Vector3(-Mathf.Sin((direction - 90) * Mathf.Deg2Rad), -Mathf.Cos((direction - 90) * Mathf.Deg2Rad)) * distance;
                Object.Instantiate(bulletPrefab, spawnPoint.position + (Vector3.up * offset.y) + deltaPos, Quaternion.Euler(new(0, 0, direction)));
            }
        }
    }
}
