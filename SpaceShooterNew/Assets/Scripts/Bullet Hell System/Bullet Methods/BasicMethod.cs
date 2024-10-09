using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMethod : BulletMethod
{
    public BasicMethod(float startTime, float duration, float frequency, BulletPattern pattern)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
    }

    protected override void Pattern()
    {
        _pattern.Spawn();
    }
}
