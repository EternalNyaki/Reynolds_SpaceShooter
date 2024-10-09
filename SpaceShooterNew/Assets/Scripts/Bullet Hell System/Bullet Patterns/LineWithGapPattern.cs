using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineWithGapPattern : LinePattern
{
    protected float _gapSize;

    public LineWithGapPattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float length, float gapPosition, float gapSize) : base(spawnPoint, bulletPrefab, direction, density, length)
    {
        this.offset.x = gapPosition;
        this._gapSize = gapSize;
    }

    public override void Spawn()
    {
        float spacing = 1 / (float)_density * _length;
        offset.x = Mathf.Clamp(offset.x, -_length / 2 + _gapSize / 2, _length / 2 - _gapSize / 2);
        for (int i = 0; i < _density; i++)
        {
            float distance = i * spacing - _length / 2;
            if (distance < offset.x - _gapSize / 2 || distance > offset.x + _gapSize / 2)
            {
                Vector3 deltaPos = new Vector3(-Mathf.Sin((direction - 90) * Mathf.Deg2Rad), -Mathf.Cos((direction - 90) * Mathf.Deg2Rad)) * distance;
                Object.Instantiate(_bulletPrefab, _spawnPoint.position + (Vector3.up * offset.y) + deltaPos, Quaternion.Euler(new(0, 0, direction)));
            }
        }
    }
}
