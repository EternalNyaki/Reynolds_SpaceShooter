using System.Collections;
using System.Collections.Generic;
using Codice.ThemeImages;
using UnityEngine;

//Event for making patterns rotate towards a target
[EventType(EventType.Direction)]
public class TargetedEvent : BulletEvent
{
    protected Transform _target;

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
