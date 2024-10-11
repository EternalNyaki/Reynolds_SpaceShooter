using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pattern for spawning bullets in a line perpendicular to their direction
public class LinePattern : BulletPattern
{
    //Density of the line (in bullets/unit)
    protected int _density;
    //Length of the line (in units)
    protected float _length;

    public LinePattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float length)
    {
        this._spawnPoint = spawnPoint;
        this._bulletPrefab = bulletPrefab;
        this.direction = direction;
        this._density = density;
        this._length = length;
    }

    public override void Spawn()
    {
        float spacing = 1 / (float)_density;
        for (int i = 0; i < _density * _length; i++)
        {
            float distance = i * spacing - _length / 2;
            Vector3 deltaPos = new Vector3(-Mathf.Sin((direction - 90) * Mathf.Deg2Rad), Mathf.Cos((direction - 90) * Mathf.Deg2Rad)) * distance;
            Object.Instantiate(_bulletPrefab, _spawnPoint.position + (Vector3)offset + deltaPos, Quaternion.Euler(new(0, 0, direction)));
        }
    }
}
