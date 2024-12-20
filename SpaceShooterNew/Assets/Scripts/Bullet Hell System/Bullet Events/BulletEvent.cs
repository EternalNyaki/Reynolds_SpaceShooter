using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Describes whether an event influences the direction or position of the underlying pattern, 
/// to ensure two patterns that influence the same field cannot be combined
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

[Serializable]
public enum EventType
{
    Basic,
    Spiral,
    Targeted,
    RandomDirection,
    RandomPosition
}

[Serializable]
public class EventTypeAttribute : Attribute
{
    public EventType type;

    public EventTypeAttribute(EventType type)
    {
        this.type = type;
    }
}

//Underlying class for all bullet events
//Bullet events control both the timing of the spawning of the underlying pattern, as well as any changes to its Fields over time
public abstract class BulletEvent
{
    //How long the event should wait to start after being run (in seconds)
    protected float _startTime;
    //How long the event should last (in seconds)
    protected float _duration;
    //Amount of time between pattern spawns (in seconds)
    protected float _interval;

    //The pattern to be spawned
    protected BulletPattern _pattern;

    [Serializable]
    public class SerializableBulletEventData
    {

        public EventType eventType;
        public float startTime;
        public float duration;
        public float interval;
        public BulletPattern.SerializableBulletPatternData pattern;
        public KeyValuePair<string, object>[] additionalParams;

    }

    [Serializable]
    public class SerializableBulletEventData<T> : SerializableBulletEventData where T : BulletEvent
    {
        public SerializableBulletEventData(float startTime, float duration, float interval, BulletPattern.SerializableBulletPatternData pattern, KeyValuePair<string, object>[] additionalParams)
        {
            eventType = typeof(T).GetCustomAttribute<EventTypeAttribute>().type;

            List<FieldInfo> eventTypeFields = new List<FieldInfo>();
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (typeof(BulletEvent).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                        .ToList().TrueForAll((FieldInfo f) => f.Name != field.Name) && field.FieldType != typeof(Transform))
                {
                    eventTypeFields.Add(field);
                }
            }
            if (additionalParams.Length != eventTypeFields.Count) throw new ArgumentException($"Constructor arguments do not match the number of arguments of type {typeof(T).Name}");

            this.startTime = startTime;
            this.duration = duration;
            this.interval = interval;
            this.pattern = pattern;
            this.additionalParams = additionalParams;
        }

        public SerializableBulletEventData(float startTime, float duration, float interval, BulletPattern.SerializableBulletPatternData pattern, params object[] additionalParams)
        {
            eventType = typeof(T).GetCustomAttribute<EventTypeAttribute>().type;

            List<FieldInfo> eventTypeFields = new List<FieldInfo>();
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (typeof(BulletEvent).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                        .ToList().TrueForAll((FieldInfo f) => f.Name != field.Name) && field.FieldType != typeof(Transform))
                {
                    eventTypeFields.Add(field);
                }
            }
            if (additionalParams.Length != eventTypeFields.Count) throw new ArgumentException($"Constructor arguments do not match the number of arguments of type {typeof(T).Name}");

            this.startTime = startTime;
            this.duration = duration;
            this.interval = interval;
            this.pattern = pattern;
            List<KeyValuePair<string, object>> temp = new List<KeyValuePair<string, object>>();
            for (int i = 0; i < additionalParams.Length; i++)
            {
                if (additionalParams[i].GetType() != eventTypeFields[i].FieldType)
                {
                    throw new ArgumentException($"Constructor arguments do not match the types of arguments of type {typeof(T).Name}");
                }

                temp.Add(new KeyValuePair<string, object>(eventTypeFields[i].Name, additionalParams[i]));
            }
            this.additionalParams = temp.ToArray();
        }
    }

    //For alterations to the base Fields of the pattern
    //Called before the pattern is spawned
    protected virtual void AlterPattern() { }

    //Coroutine to run the event
    public IEnumerator Run()
    {
        if (_startTime != 0)
        {
            yield return new WaitForSeconds(_startTime);
        }

        WaitForSeconds waitForInterval = new WaitForSeconds(_interval);
        float timeAtStart = Time.time;
        //This is a do-while instead of a standard while to guarantee the pattern will always run at least once even with a duration of 0
        do
        {
            AlterPattern();
            _pattern.Spawn();
            yield return waitForInterval;
        } while (timeAtStart + _duration >= Time.time);
    }

    //HACK: For testing, method should be removed for the final product
#if UNITY_EDITOR
    public void SetPattern(BulletPattern pattern)
    {
        _pattern = pattern;
    }
#endif

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

    public static BulletEvent FromEventData(SerializableBulletEventData data, Transform managerTransform = null, Transform targetTransform = null, GameObject bulletPrefab = null)
    {
        BulletPattern pattern;
        //TODO: Add/change bullet type logic
        switch (data.pattern.patternType)
        {
            case PatternType.Single:
                pattern = new SinglePattern(managerTransform, data.pattern.spawnPoint, bulletPrefab, data.pattern.direction);
                break;

            case PatternType.Ring:
                pattern = new RingPattern(managerTransform, data.pattern.spawnPoint, bulletPrefab, data.pattern.direction, Convert.ToInt32(data.pattern.additionalParams[0].Value));
                break;

            case PatternType.RingWithGap:
                pattern = new RingWithGapPattern(managerTransform, data.pattern.spawnPoint, bulletPrefab, data.pattern.direction, Convert.ToInt32(data.pattern.additionalParams[1].Value), Convert.ToSingle(data.pattern.additionalParams[0].Value));
                break;

            case PatternType.Line:
                pattern = new LinePattern(managerTransform, data.pattern.spawnPoint, bulletPrefab, data.pattern.direction, Convert.ToInt32(data.pattern.additionalParams[0].Value), Convert.ToSingle(data.pattern.additionalParams[1].Value));
                break;

            case PatternType.LineWithGap:
                pattern = new LineWithGapPattern(managerTransform, data.pattern.spawnPoint, bulletPrefab, data.pattern.direction, Convert.ToInt32(data.pattern.additionalParams[1].Value), Convert.ToSingle(data.pattern.additionalParams[2].Value), data.pattern.spawnPoint.x, Convert.ToSingle(data.pattern.additionalParams[0].Value));
                break;

            default:
                throw new ArgumentException("Invalid pattern type");
        }

        BulletEvent output;
        switch (data.eventType)
        {
            case EventType.Basic:
                output = new BasicEvent(data.startTime, data.duration, data.interval, pattern);
                break;

            case EventType.Spiral:
                output = new SpiralEvent(data.startTime, data.duration, data.interval, pattern, Convert.ToSingle(data.additionalParams[0].Value));
                break;

            case EventType.Targeted:
                output = new TargetedEvent(data.startTime, data.duration, data.interval, pattern, targetTransform);
                break;

            case EventType.RandomDirection:
                output = new RandomDirectionEvent(data.startTime, data.duration, data.interval, pattern, Convert.ToSingle(data.additionalParams[0].Value), Convert.ToSingle(data.additionalParams[1].Value));
                break;

            case EventType.RandomPosition:
                output = new RandomPositionEvent(data.startTime, data.duration, data.interval, pattern, Convert.ToSingle(data.additionalParams[0].Value), Convert.ToSingle(data.additionalParams[1].Value), (RectTransform.Axis)Convert.ToInt32(data.additionalParams[2].Value));
                break;

            default:
                throw new ArgumentException("Invalid event type");
        }

        return output;
    }

    public static List<SerializableBulletEventData> DeserializeBulletEventArray(string json)
    {
        List<SerializableBulletEventData> temp = UnityNewtonsoftJsonSerializer.instance.Deserialize<List<SerializableBulletEventData>>(json);
        List<SerializableBulletEventData> output = new List<SerializableBulletEventData>();

        foreach (SerializableBulletEventData bEvent in temp)
        {
            switch (bEvent.pattern.patternType)
            {
                case PatternType.Single:
                    bEvent.pattern = new BulletPattern.SerializableBulletPatternData<SinglePattern>(bEvent.pattern.spawnPoint, bEvent.pattern.bulletType, bEvent.pattern.direction, bEvent.pattern.additionalParams);
                    break;

                case PatternType.Ring:
                    bEvent.pattern = new BulletPattern.SerializableBulletPatternData<RingPattern>(bEvent.pattern.spawnPoint, bEvent.pattern.bulletType, bEvent.pattern.direction, bEvent.pattern.additionalParams);
                    break;

                case PatternType.RingWithGap:
                    bEvent.pattern = new BulletPattern.SerializableBulletPatternData<RingWithGapPattern>(bEvent.pattern.spawnPoint, bEvent.pattern.bulletType, bEvent.pattern.direction, bEvent.pattern.additionalParams);
                    break;

                case PatternType.Line:
                    bEvent.pattern = new BulletPattern.SerializableBulletPatternData<LinePattern>(bEvent.pattern.spawnPoint, bEvent.pattern.bulletType, bEvent.pattern.direction, bEvent.pattern.additionalParams);
                    break;

                case PatternType.LineWithGap:
                    bEvent.pattern = new BulletPattern.SerializableBulletPatternData<LineWithGapPattern>(bEvent.pattern.spawnPoint, bEvent.pattern.bulletType, bEvent.pattern.direction, bEvent.pattern.additionalParams);
                    break;
            }

            switch (bEvent.eventType)
            {
                case EventType.Basic:
                    output.Add(new SerializableBulletEventData<BasicEvent>(bEvent.startTime, bEvent.duration, bEvent.interval, bEvent.pattern, bEvent.additionalParams));
                    break;

                case EventType.Spiral:
                    output.Add(new SerializableBulletEventData<SpiralEvent>(bEvent.startTime, bEvent.duration, bEvent.interval, bEvent.pattern, bEvent.additionalParams));
                    break;

                case EventType.Targeted:
                    output.Add(new SerializableBulletEventData<TargetedEvent>(bEvent.startTime, bEvent.duration, bEvent.interval, bEvent.pattern, bEvent.additionalParams));
                    break;

                case EventType.RandomDirection:
                    output.Add(new SerializableBulletEventData<RandomDirectionEvent>(bEvent.startTime, bEvent.duration, bEvent.interval, bEvent.pattern, bEvent.additionalParams));
                    break;

                case EventType.RandomPosition:
                    output.Add(new SerializableBulletEventData<RandomPositionEvent>(bEvent.startTime, bEvent.duration, bEvent.interval, bEvent.pattern, bEvent.additionalParams));
                    break;
            }
        }

        return output;
    }
}
