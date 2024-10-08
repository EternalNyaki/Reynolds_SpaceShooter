using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineWithGapPattern : LinePattern
{
    float _gapPosition;
    float _gapSize;

    public LineWithGapPattern(Transform spawnPoint, GameObject bulletPrefab, float direction, int density, float length, float gapPosition, float gapSize) : base(spawnPoint, bulletPrefab, direction, density, length)
    {
        _gapPosition = gapPosition;
        _gapSize = gapSize;
    }

    public override void Spawn()
    {
        float spacing = 1 / (float)_density * _length;
        for (int i = 0; i < _density; i++)
        {
            float distance = i * spacing - _length / 2;
            if (distance < _gapPosition - _gapSize / 2 || distance > _gapPosition + _gapSize / 2)
            {
                Vector3 deltaPos = new Vector3(-Mathf.Sin((_direction - 90) * Mathf.Deg2Rad), -Mathf.Cos((_direction - 90) * Mathf.Deg2Rad)) * distance;
                Object.Instantiate(_bulletPrefab, _spawnPoint.position + deltaPos, Quaternion.Euler(new(0, 0, _direction)));
            }
        }
    }
}
