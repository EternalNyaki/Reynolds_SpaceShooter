using System;
using UnityEngine;

[MethodType(MethodType.Position)]
public class RandomPositionMethod : BulletMethod
{
    protected Func<float> _randomFunction;

    protected RectTransform.Axis _axis;

    public RandomPositionMethod(float startTime, float duration, float frequency, BulletPattern pattern, float minDistance, float maxDistance, RectTransform.Axis axis)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _randomFunction = () => { return UnityEngine.Random.Range(minDistance, maxDistance); };
        _axis = axis;
    }

    public RandomPositionMethod(float startTime, float duration, float frequency, BulletPattern pattern, Func<float> randomFunction, RectTransform.Axis axis)
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
        ;
    }
}
