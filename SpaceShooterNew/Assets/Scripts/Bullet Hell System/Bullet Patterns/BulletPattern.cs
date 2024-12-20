using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

[Serializable]
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
    public Vector2 offset;

    //The type of bullet to be spawned
    public GameObject bulletPrefab;
    //The direction the bullets should be spawned relative to
    public float direction;

    [Serializable]
    public class SerializableBulletPatternData
    {
        public PatternType patternType;
        public Vector2 spawnPoint;
        public BulletType bulletType;
        public float direction;
        public KeyValuePair<string, object>[] additionalParams;
    }

    [Serializable]
    public class SerializableBulletPatternData<T> : SerializableBulletPatternData where T : BulletPattern
    {
        public SerializableBulletPatternData(Vector2 spawnPoint, BulletType bulletType, float direction, KeyValuePair<string, object>[] additionalParams)
        {
            patternType = typeof(T).GetCustomAttribute<PatternTypeAttribute>().type;

            List<FieldInfo> patternTypeFields = new List<FieldInfo>();
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (typeof(BulletPattern).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                            .ToList().TrueForAll((FieldInfo f) => f.Name != field.Name))
                {
                    patternTypeFields.Add(field);
                }
            }
            if (additionalParams.Length != patternTypeFields.Count) throw new ArgumentException($"Constructor arguments do not match the number of arguments of type {typeof(T).Name}");

            this.spawnPoint = spawnPoint;
            this.bulletType = bulletType;
            this.direction = direction;
            this.additionalParams = additionalParams;
        }

        public SerializableBulletPatternData(Vector2 spawnPoint, BulletType bulletType, float direction, params object[] additionalParams)
        {
            patternType = typeof(T).GetCustomAttribute<PatternTypeAttribute>().type;

            List<FieldInfo> patternTypeFields = new List<FieldInfo>();
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (typeof(BulletPattern).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                            .ToList().TrueForAll((FieldInfo f) => f.Name != field.Name))
                {
                    patternTypeFields.Add(field);
                }
            }
            if (additionalParams.Length != patternTypeFields.Count) throw new ArgumentException($"Constructor arguments do not match the number of arguments of type {typeof(T).Name}");

            this.spawnPoint = spawnPoint;
            this.bulletType = bulletType;
            this.direction = direction;
            List<KeyValuePair<string, object>> temp = new List<KeyValuePair<string, object>>();
            for (int i = 0; i < additionalParams.Length; i++)
            {
                if (!(additionalParams[i].GetType() == patternTypeFields[i].FieldType))
                {
                    throw new ArgumentException($"Constructor arguments do not match the types of arguments of type {typeof(T).Name}");
                }

                temp.Add(new KeyValuePair<string, object>(patternTypeFields[i].Name, additionalParams[i]));
            }
            this.additionalParams = temp.ToArray();
        }
    }

    public virtual void Spawn() { }

    public Vector2 GetSpawnPoint() => (Vector2)spawnPoint.position + offset;

    //HACK: For testing
#if UNITY_EDITOR
    public void SetBulletType(GameObject bullet)
    {
        bulletPrefab = bullet;
    }
#endif
}
