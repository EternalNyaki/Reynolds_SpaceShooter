using System;
using UnityEngine;

//Event for spawning patterns with a random direction within a given range
[EventType(EventType.Direction)]
public class RandomDirectionEvent : BulletEvent
{
    //Method for randomly choosing the direction of the pattern
    protected Func<float> _randomFunction;

    public RandomDirectionEvent(float startTime, float duration, float frequency, BulletPattern pattern, float minAngle, float maxAngle)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        //HACK: For some reason explicitly declaring that we're using the UnityEngine.Random class requires the method be given anonymously
        _randomFunction = () => { return UnityEngine.Random.Range(minAngle, maxAngle); };
    }

    public RandomDirectionEvent(float startTime, float duration, float frequency, BulletPattern pattern, Func<float> randomFunction)
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
    }
}
