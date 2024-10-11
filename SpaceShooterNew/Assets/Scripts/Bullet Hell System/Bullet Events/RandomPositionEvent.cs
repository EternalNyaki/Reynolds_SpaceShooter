using System;
using UnityEngine;

//Event for spawning patterns at a random position on a line
[EventType(EventType.Position)]
public class RandomPositionEvent : BulletEvent
{
    //Method for randomly choosing the direction of the pattern
    protected Func<float> _randomFunction;

    //Whether the position should be randomized on the horizontal of vertical axis
    protected RectTransform.Axis _axis;

    public RandomPositionEvent(float startTime, float duration, float frequency, BulletPattern pattern, float minDistance, float maxDistance, RectTransform.Axis axis)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        //HACK: For some reason explicitly declaring that we're using the UnityEngine.Random class requires the method be given anonymously
        _randomFunction = () => { return UnityEngine.Random.Range(minDistance, maxDistance); };
        _axis = axis;
    }

    public RandomPositionEvent(float startTime, float duration, float frequency, BulletPattern pattern, Func<float> randomFunction, RectTransform.Axis axis)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _randomFunction = randomFunction;
        _axis = axis;
    }

    protected internal override void AlterPattern()
    {
        Vector2 offset;
        switch (_axis)
        {
            case RectTransform.Axis.Horizontal:
                offset = Vector2.right * _randomFunction();
                break;

            case RectTransform.Axis.Vertical:
                offset = Vector2.up * _randomFunction();
                break;

            default:
                offset = Vector2.zero;
                break;
        }
        _pattern.offset = offset;
    }
}
