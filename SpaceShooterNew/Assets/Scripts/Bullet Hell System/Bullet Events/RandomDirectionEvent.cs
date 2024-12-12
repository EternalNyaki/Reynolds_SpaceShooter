using System;
using UnityEngine;

//Event for spawning patterns with a random direction within a given range
[EventDomain(EventDomain.Direction), EventType(EventType.RandomDirection)]
public class RandomDirectionEvent : BulletEvent
{
#if UNITY_EDITOR
    //Public interface to properties for custom inspector
    //For all functional purposes these DO NOT EXIST

    /// <summary>
    /// EDITOR-ONLY interface for minimum angle (in degrees)
    /// </summary>
    public float minAngle
    {
        get { return _minAngle; }
        set { _minAngle = value; }
    }

    /// <summary>
    /// EDITOR-ONLY interface for maximum angle (in degrees)
    /// </summary>
    public float maxAngle
    {
        get { return _maxAngle; }
        set { _maxAngle = value; }
    }
#endif

    protected float _minAngle;
    protected float _maxAngle;

    public RandomDirectionEvent()
    {
        _startTime = 0f;
        _duration = 0f;
        _interval = 0f;
        _pattern = null;
        _minAngle = 0f;
        _maxAngle = 0f;
    }

    public RandomDirectionEvent(float startTime, float duration, float frequency, BulletPattern pattern, float minAngle, float maxAngle)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _minAngle = minAngle;
        _maxAngle = maxAngle;
    }

    protected internal override void AlterPattern()
    {
        _pattern.direction = UnityEngine.Random.Range(_minAngle, _maxAngle);
    }
}
