using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event for spawning patterns unaltered
[EventDomain(EventDomain.None), EventType(EventType.Basic)]
public class BasicEvent : BulletEvent
{
    public BasicEvent()
    {
        _startTime = 0f;
        _duration = 0f;
        _interval = 0f;
        _pattern = null;
    }

    public BasicEvent(float startTime, float duration, float interval, BulletPattern pattern)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = interval;
        _pattern = pattern;
    }
}
