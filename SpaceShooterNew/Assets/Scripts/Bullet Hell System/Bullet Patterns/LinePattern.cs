using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawning bullets in a line perpendicular to their direction
[PatternType(PatternType.Line)]
public class LinePattern : BulletPattern
{
    //Density of the line (in bullets/unit)
    public int density;
    //Length of the line (in units)
    public float length;

    public LinePattern()
    {
        spawnPoint = null;
        bulletPrefab = null;
        direction = 0f;
        density = 0;
        length = 0f;
    }

    public LinePattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float length)
    {
        this.spawnPoint = spawnPoint;
        this.bulletPrefab = bulletPrefab;
        this.direction = direction;
        this.density = density;
        this.length = length;
    }

    public override void Spawn()
    {
        float spacing = 1 / (float)density;
        for (int i = 0; i < density * length; i++)
        {
            float distance = i * spacing - length / 2;
            Vector3 deltaPos = new Vector3(-Mathf.Sin((direction - 90) * Mathf.Deg2Rad), Mathf.Cos((direction - 90) * Mathf.Deg2Rad)) * distance;
            Object.Instantiate(bulletPrefab, spawnPoint.position + (Vector3)offset + deltaPos, Quaternion.Euler(new(0, 0, direction)));
        }
    }
}
