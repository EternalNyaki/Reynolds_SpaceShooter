using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all bullet patterns
//Patterns dictate how bullets are spawned relative to each other on one frame
public abstract class BulletPattern
{
    //The position from which the bullets should be spawned
    protected Transform _spawnPoint;
    //The offset of the bullets from the spawn point
    public Vector2 offset = Vector2.zero;

    //The type of bullet to be spawned
    protected GameObject _bulletPrefab;
    //The direction the bullets should be spawned relative to
    public float direction;

    public virtual void Spawn()
    {

    }

    public Vector2 GetSpawnPoint()
    {
        return (Vector2)_spawnPoint.position + offset;
    }
}
