using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatternType
{
    Single,
    Ring,
    RingWithGap,
    Line,
    LineWithGap
}

public class PatternTypeAttribute : Attribute
{
    public PatternType type;

    public PatternTypeAttribute(PatternType type)
    {
        this.type = type;
    }
}

//Base class for all bullet patterns
//Patterns dictate how bullets are spawned relative to each other on one frame
public abstract class BulletPattern
{
    //The position from which the bullets should be spawned
    public Transform spawnPoint;
    //The offset of the bullets from the spawn point
    public Vector2 offset = Vector2.zero;

    //The type of bullet to be spawned
    public GameObject bulletPrefab;
    //The direction the bullets should be spawned relative to
    public float direction;

    public virtual void Spawn()
    {

    }

    public Vector2 GetSpawnPoint()
    {
        return (Vector2)spawnPoint.position + offset;
    }

    //HACK: For testing
    public void SetBulletType(GameObject bullet)
    {
        bulletPrefab = bullet;
    }
}
