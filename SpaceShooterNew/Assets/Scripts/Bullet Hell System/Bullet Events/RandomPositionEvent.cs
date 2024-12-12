using System;
using UnityEngine;

//Event for spawning patterns at a random position on a line
[EventDomain(EventDomain.Position), EventType(EventType.RandomPosition)]
public class RandomPositionEvent : BulletEvent
{
#if UNITY_EDITOR
    //Public interface to properties for custom inspector
    //For all functional purposes these DO NOT EXIST

    /// <summary>
    /// EDITOR-ONLY interface for minimum distance (in units)
    /// </summary>
    public float minDistance
    {
        get { return _minDistance; }
        set { _minDistance = value; }
    }

    /// <summary>
    /// EDITOR-ONLY interface for maximum distance (in units)
    /// </summary>
    public float maxDistance
    {
        get { return _maxDistance; }
        set { _maxDistance = value; }
    }

    /// <summary>
    /// EDITOR-ONLY interface for axis
    /// </summary>
    public RectTransform.Axis axis
    {
        get { return _axis; }
        set { _axis = value; }
    }
#endif

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

    public RandomPositionEvent(float startTime, float duration, float frequency, BulletPattern pattern, float minDistance, float maxDistance, RectTransform.Axis axis)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _minDistance = minDistance;
        _maxDistance = maxDistance;
        _axis = axis;
    }

    protected internal override void AlterPattern()
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
