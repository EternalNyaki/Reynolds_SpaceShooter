using System;
using UnityEngine;

//Event for spawning patterns with a random direction within a given range
[EventDomain(EventDomain.Direction), EventType(EventType.RandomDirection)]
public class RandomDirectionEvent : BulletEvent
{
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

    public RandomDirectionEvent(float startTime, float duration, float interval, BulletPattern pattern, float minAngle, float maxAngle)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = interval;
        _pattern = pattern;
        _minAngle = minAngle;
        _maxAngle = maxAngle;
    }

    protected override void AlterPattern()
    {
        _pattern.direction = UnityEngine.Random.Range(_minAngle, _maxAngle);
    }
}
