using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public enum MethodType
{
    None,
    Direction,
    Position,
    Both
}

public class MethodTypeAttribute : Attribute
{
    public MethodType type;

    public MethodTypeAttribute(MethodType type)
    {
        this.type = type;
    }
}

public abstract class BulletMethod
{
    protected internal float _startTime;
    protected internal float _duration;
    protected internal float _interval;
    protected internal BulletPattern _pattern;

    protected internal virtual void AlterPattern()
    {

    }

    public IEnumerator Run()
    {
        if (_startTime != 0)
        {
            yield return new WaitForSeconds(_startTime);
        }

        float timeAtStart = Time.time;
        //This is a do-while instead of a standard while to allow a duration of 0 to have the pattern run exactly once
        do
        {
            AlterPattern();
            _pattern.Spawn();
            yield return new WaitForSeconds(_interval);
        } while (timeAtStart + _duration >= Time.time);
    }

    //For testing, will be removed for the final product
    public void SetPattern(BulletPattern pattern)
    {
        _pattern = pattern;
    }

    public static BulletMethod CloneBaseValues(BulletMethod methodToClone, BulletMethod cloningTarget)
    {
        cloningTarget._startTime = methodToClone._startTime;
        cloningTarget._duration = methodToClone._duration;
        cloningTarget._interval = methodToClone._interval;
        cloningTarget._pattern = methodToClone._pattern;
        return cloningTarget;
    }

    public static CombinedMethod CombineMethods(BulletMethod method1, BulletMethod method2)
    {
        MethodType m1Type = method1.GetType().GetCustomAttribute<MethodTypeAttribute>().type;
        MethodType m2Type = method2.GetType().GetCustomAttribute<MethodTypeAttribute>().type;

        if (!AreBasePropertiesEqual(method1, method2))
        {
            Debug.LogError("Cannot combine methods: Methods have different base properties. Use BulletMethod.CloneBaseValues to give the second method the properties of the first");
        }
        if (m1Type == MethodType.None || m2Type == MethodType.None)
        {
            Debug.LogWarning("Cannot combine methods: One or both methods have a type of None.");
        }
        else if (m1Type == m2Type)
        {
            Debug.LogWarning("Cannot combine methods: Both methods have the same type");
        }
        else if (m1Type == MethodType.Both || m2Type == MethodType.Both)
        {
            Debug.LogWarning("Cannot combine methods: One of both methods have a type of Both.");
        }
        else if ((m1Type == MethodType.Direction && m2Type == MethodType.Position) || (m1Type == MethodType.Position && m2Type == MethodType.Direction))
        {
            return new CombinedMethod(method1, method2);
        }

        return null;
    }

    public static bool AreBasePropertiesEqual(BulletMethod method1, BulletMethod method2)
    {
        return (method1._startTime == method2._startTime) && (method1._duration == method2._duration) && (method1._interval == method2._interval) && (method1._pattern == method2._pattern);
    }
}

[MethodType(MethodType.Both)]
public class CombinedMethod : BulletMethod
{
    BulletMethod _method1;
    BulletMethod _method2;

    public CombinedMethod(BulletMethod method1, BulletMethod method2)
    {
        if (!AreBasePropertiesEqual(method1, method2))
        {
            throw new ArgumentException("Cannot combine methods that don't have the same base properties.");
        }
        _startTime = method1._startTime;
        _duration = method1._duration;
        _interval = method1._interval;
        _pattern = method1._pattern;
        _method1 = method1;
        _method2 = method2;
    }

    protected internal override void AlterPattern()
    {
        _method1.AlterPattern();
        _method2.AlterPattern();
    }
}
