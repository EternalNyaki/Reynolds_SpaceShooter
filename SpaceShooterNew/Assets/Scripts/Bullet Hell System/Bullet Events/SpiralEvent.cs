using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event for changing the direction of patterns by a consistent amount each time they are spawned
[EventDomain(EventDomain.Direction), EventType(EventType.Spiral)]
public class SpiralEvent : BulletEvent
{
#if UNITY_EDITOR
    //Public interface to properties for custom inspector
    //For all functional purposes these DO NOT EXIST

    /// <summary>
    /// EDITOR-ONLY interface for change in angle (in degrees)
    /// </summary>
    public float deltaAngle
    {
        get { return _deltaAngle; }
        set { _deltaAngle = value; }
    }
#endif

    //Amount to rotate the pattern by each cycle
    protected float _deltaAngle;

    public SpiralEvent()
    {
        _startTime = 0f;
        _duration = 0f;
        _interval = 0f;
        _pattern = null;
        _deltaAngle = 0f;
    }

    public SpiralEvent(float startTime, float duration, float frequency, BulletPattern pattern, float deltaAngle)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _deltaAngle = deltaAngle;

        //Rotating the pattern so that it's direction will be its set direction the first time it's spawned (instead of it's direction + deltaAngle)
        _pattern.direction -= _deltaAngle;
    }

    protected internal override void AlterPattern()
    {
        _pattern.direction += _deltaAngle;
    }
}
