using System;
using UnityEngine;

//Event for spawning patterns at a random position on a line
[EventDomain(EventDomain.Position), EventType(EventType.RandomPosition)]
public class RandomPositionEvent : BulletEvent
{
    protected float _minDistance;
    protected float _maxDistance;

    //Whether the position should be randomized on the horizontal of vertical axis
    protected RectTransform.Axis _axis;

    public RandomPositionEvent()
    {
        _startTime = 0f;
        _duration = 0f;
        _interval = 0f;
        _pattern = null;
        _maxDistance = 0f;
        _minDistance = 0f;
        _axis = RectTransform.Axis.Horizontal;
    }

    public RandomPositionEvent(float startTime, float duration, float interval, BulletPattern pattern, float minDistance, float maxDistance, RectTransform.Axis axis)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = interval;
        _pattern = pattern;
        _minDistance = minDistance;
        _maxDistance = maxDistance;
        _axis = axis;
    }

    protected override void AlterPattern()
    {
        Vector2 offset;
        switch (_axis)
        {
            case RectTransform.Axis.Horizontal:
                offset = Vector2.right * UnityEngine.Random.Range(_minDistance, _maxDistance);
                break;

            case RectTransform.Axis.Vertical:
                offset = Vector2.up * UnityEngine.Random.Range(_minDistance, _maxDistance);
                break;

            default:
                offset = Vector2.zero;
                break;
        }
        _pattern.offset = offset;
    }
}
