using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event for changing the direction of patterns by a consistent amount each time they are spawned
[EventType(EventType.Direction)]
public class SpiralEvent : BulletEvent
{
    //Amount to rotate the pattern by each cycle
    protected float _deltaAngle;

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
