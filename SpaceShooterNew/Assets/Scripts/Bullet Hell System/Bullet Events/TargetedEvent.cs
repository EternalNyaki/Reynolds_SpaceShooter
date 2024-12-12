using System.Collections;
using System.Collections.Generic;
using Codice.ThemeImages;
using UnityEngine;

//Event for making patterns rotate towards a target
[EventDomain(EventDomain.Direction), EventType(EventType.Targeted)]
public class TargetedEvent : BulletEvent
{
#if UNITY_EDITOR
    //Public interface to properties for custom inspector
    //For all functional purposes these DO NOT EXIST

    /// <summary>
    /// EDITOR-ONLY interface for target transform
    /// </summary>
    public Transform target
    {
        get { return _target; }
        set { _target = value; }
    }
#endif

    protected Transform _target;

    public TargetedEvent()
    {
        _startTime = 0f;
        _duration = 0f;
        _interval = 0f;
        _pattern = null;
        _target = null;
    }

    public TargetedEvent(float startTime, float duration, float frequency, BulletPattern pattern, Transform target)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
        _target = target;
    }

    protected internal override void AlterPattern()
    {
        float angleToTarget = Mathf.Atan2(_target.position.y - _pattern.GetSpawnPoint().y, _target.position.x - _pattern.GetSpawnPoint().x) * Mathf.Rad2Deg + 90;
        _pattern.direction = angleToTarget;
    }
}
