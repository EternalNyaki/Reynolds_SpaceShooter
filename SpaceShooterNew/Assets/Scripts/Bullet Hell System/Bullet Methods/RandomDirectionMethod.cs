using System;
using UnityEngine;

[MethodType(MethodType.Direction)]
public class RandomDirectionMethod : BulletMethod
{
    protected Func<float> _randomFunction;

    public RandomDirectionMethod(float startTime, float duration, float frequency, BulletPattern pattern, float minAngle, float maxAngle)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _randomFunction = () => { return UnityEngine.Random.Range(minAngle, maxAngle); };
    }

    public RandomDirectionMethod(float startTime, float duration, float frequency, BulletPattern pattern, Func<float> randomFunction)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _randomFunction = randomFunction;
    }

    protected internal override void AlterPattern()
    {
        _pattern.direction = _randomFunction();
        ;
    }
}
