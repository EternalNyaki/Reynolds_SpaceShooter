using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralMethod : BulletMethod
{
    protected float _deltaAngle;

    public SpiralMethod(float startTime, float duration, float frequency, BulletPattern pattern, float deltaAngle)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _deltaAngle = deltaAngle;
    }

    protected override void Pattern()
    {
        _pattern.Spawn();
        _pattern.direction += _deltaAngle;
    }
}
