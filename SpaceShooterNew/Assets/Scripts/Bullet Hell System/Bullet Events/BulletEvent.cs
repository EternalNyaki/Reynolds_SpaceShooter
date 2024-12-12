using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Describes whether an event influences the direction or position of the underlying pattern, to ensure two patterns that influence the same property cannot be combined
/// </summary>
public enum EventDomain
{
    None,
    Direction,
    Position,
    Both
}

public class EventDomainAttribute : Attribute
{
    public EventDomain type;

    public EventDomainAttribute(EventDomain type)
    {
        this.type = type;
    }
}


public enum EventType
{
    Basic,
    Spiral,
    Targeted,
    RandomDirection,
    RandomPosition
}

public class EventTypeAttribute : Attribute
{
    public EventType type;

    public EventTypeAttribute(EventType type)
    {
        this.type = type;
    }
}

//Underlying class for all bullet events
//Bullet events control both the timing of the spawning of the underlying pattern, as well as any changes to its properties over time
public abstract class BulletEvent
{
#if UNITY_EDITOR
    //Public interface to properties for custom inspector
    //For all functional purposes these DO NOT EXIST

    /// <summary>
    /// EDITOR-ONLY interface for start time (in seconds)
    /// </summary>
    public float startTime
    {
        get { return _startTime; }
        set { _startTime = value; }
    }

    /// <summary>
    /// EDITOR-ONLY interface for duration (in seconds)
    /// </summary>
    public float duration
    {
        get { return _duration; }
        set { _duration = value; }
    }

    /// <summary>
    /// EDITOR-ONLY interface for frequency (cycles per second)
    /// </summary>
    public float frequency
    {
        get { return 1 / _interval; }
        set { _interval = 1 / frequency; }
    }

    /// <summary>
    /// EDITOR-ONLY interface for bullet pattern
    /// </summary>
    public BulletPattern pattern
    {
        get { return _pattern; }
        set { _pattern = value; }
    }
#endif

    //How long the event should wait to start after being run (in seconds)
    protected internal float _startTime;
    //How long the event should last (in seconds)
    protected internal float _duration;
    //Amount of time between pattern spawns (in seconds)
    protected internal float _interval;

    //The pattern to be spawned
    protected internal BulletPattern _pattern;

    //For alterations to the base properties of the pattern
    //Called before the pattern is spawned
    protected internal virtual void AlterPattern()
    {

    }

    //Coroutine to run the event
    public IEnumerator Run()
    {
        if (_startTime != 0)
        {
            yield return new WaitForSeconds(_startTime);
        }

        float timeAtStart = Time.time;
        //This is a do-while instead of a standard while to allow a duration of 0 to have the pattern run exactly once
        do
        {
            AlterPattern();
            _pattern.Spawn();
            yield return new WaitForSeconds(_interval);
        } while (timeAtStart + _duration >= Time.time);
    }

    //HACK: For testing, method should be removed for the final product
    public void SetPattern(BulletPattern pattern)
    {
        _pattern = pattern;
    }

    /// <summary>
    /// Set the base values (start time, duration, interval, and pattern) of the cloningTarget to those of the eventToClone
    /// </summary>
    /// <param name="eventToClone"> Event who's base values should be used </param>
    /// <param name="cloningTarget"> Event who's base values should be set </param>
    /// <returns> The cloningTarget with updated base values </returns>
    public static BulletEvent CloneBaseValues(BulletEvent eventToClone, BulletEvent cloningTarget)
    {
        cloningTarget._startTime = eventToClone._startTime;
        cloningTarget._duration = eventToClone._duration;
        cloningTarget._interval = eventToClone._interval;
        cloningTarget._pattern = eventToClone._pattern;
        return cloningTarget;
    }

    /// <summary>
    /// Created a CombinedEvent with the properties of both events
    /// Events must both have different EventDomains (direction or position) and have the same base values
    /// </summary>
    /// <param name="event1"></param>
    /// <param name="event2"></param>
    /// <returns> A CombinedEvent with the properties of both events </returns>
    public static CombinedEvent CombineEvents(BulletEvent event1, BulletEvent event2)
    {
        EventDomain m1Type = event1.GetType().GetCustomAttribute<EventDomainAttribute>().type;
        EventDomain m2Type = event2.GetType().GetCustomAttribute<EventDomainAttribute>().type;

        if (!AreBasePropertiesEqual(event1, event2))
        {
            Debug.LogError("Cannot combine Events: Events have different base properties. Use BulletEvent.CloneBaseValues to give the second Event the properties of the first");
        }
        if (m1Type == EventDomain.None || m2Type == EventDomain.None)
        {
            Debug.LogWarning("Cannot combine Events: One or both Events have a type of None.");
        }
        else if (m1Type == m2Type)
        {
            Debug.LogWarning("Cannot combine Events: Both Events have the same type");
        }
        else if (m1Type == EventDomain.Both || m2Type == EventDomain.Both)
        {
            Debug.LogWarning("Cannot combine Events: One of both Events have a type of Both.");
        }
        else if ((m1Type == EventDomain.Direction && m2Type == EventDomain.Position) || (m1Type == EventDomain.Position && m2Type == EventDomain.Direction))
        {
            return new CombinedEvent(event1, event2);
        }

        return null;
    }

    /// <summary>
    /// Checks the base properties (start time, duration, interval, and pattern) of both events and returns true if they are equal
    /// </summary>
    /// <param name="event1"></param>
    /// <param name="event2"></param>
    /// <returns> true if the base properties of both events are the same, false otherwise </returns>
    public static bool AreBasePropertiesEqual(BulletEvent event1, BulletEvent event2)
    {
        return (event1._startTime == event2._startTime) && (event1._duration == event2._duration) && (event1._interval == event2._interval) && (event1._pattern == event2._pattern);
    }
}

//Class for a bullet event that is a combination of two other event types
[EventDomain(EventDomain.Both)]
public class CombinedEvent : BulletEvent
{
    BulletEvent _event1;
    BulletEvent _event2;

    public CombinedEvent(BulletEvent event1, BulletEvent event2)
    {
        if (!AreBasePropertiesEqual(event1, event2))
        {
            throw new ArgumentException("Cannot combine Events that don't have the same base properties.");
        }
        _startTime = event1._startTime;
        _duration = event1._duration;
        _interval = event1._interval;
        _pattern = event1._pattern;
        _event1 = event1;
        _event2 = event2;
    }

    protected internal override void AlterPattern()
    {
        _event1.AlterPattern();
        _event2.AlterPattern();
    }
}
