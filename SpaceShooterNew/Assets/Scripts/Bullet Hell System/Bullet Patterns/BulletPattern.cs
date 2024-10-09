using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BulletPattern
{
    protected Transform _spawnPoint;
    protected GameObject _bulletPrefab;
    public float direction;
    public Vector2 offset = Vector2.zero;

    public virtual void Spawn()
    {

    }

    public Vector2 GetSpawnPoint()
    {
        return (Vector2)_spawnPoint.position + offset;
    }
}
