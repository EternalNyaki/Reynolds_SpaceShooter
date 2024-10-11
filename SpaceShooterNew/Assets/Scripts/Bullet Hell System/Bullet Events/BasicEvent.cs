using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event for spawning patterns unaltered
[EventType(EventType.None)]
public class BasicEvent : BulletEvent
{
    public BasicEvent(float startTime, float duration, float frequency, BulletPattern pattern)
    {
        _startTime = startTime;
        _duration = duration;
        _interval = 1 / frequency;
        _pattern = pattern;
    }
}
