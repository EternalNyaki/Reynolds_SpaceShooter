using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BulletPattern
{
    protected Transform _spawnPoint;
    protected GameObject _bulletPrefab;
    protected float _direction;

    public virtual void Spawn()
    {

    }
}
