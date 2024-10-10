using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MethodType(MethodType.Direction)]
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

    protected internal override void AlterPattern()
    {
        ;
        _pattern.direction += _deltaAngle;
    }
}
