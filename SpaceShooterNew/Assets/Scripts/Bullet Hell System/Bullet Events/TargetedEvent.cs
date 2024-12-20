using System.Collections;
using System.Collections.Generic;
using Codice.ThemeImages;
using UnityEngine;

//Event for making patterns rotate towards a target
[EventDomain(EventDomain.Direction), EventType(EventType.Targeted)]
public class TargetedEvent : BulletEvent
{
    protected Transform _target;

    public TargetedEvent()
    {
        _startTime = 0f;
        _duration = 0f;
        _interval = 0f;
        _pattern = null;
        _target = null;
    }

    public TargetedEvent(float startTime, float duration, float interval, BulletPattern pattern, Transform target)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = interval;
        _pattern = pattern;
        _target = target;
    }

    protected override void AlterPattern()
    {
        float angleToTarget = Mathf.Atan2(_target.position.y - _pattern.GetSpawnPoint().y, _target.position.x - _pattern.GetSpawnPoint().x) * Mathf.Rad2Deg + 90;
        _pattern.direction = angleToTarget;
    }
}
