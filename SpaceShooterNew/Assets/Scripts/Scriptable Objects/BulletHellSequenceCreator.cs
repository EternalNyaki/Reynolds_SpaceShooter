using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using System;
using Unity.Plastic.Newtonsoft.Json;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Bullet Hell Sequence Creator", menuName = "ScriptableObjects/BulletHellSequenceCreator", order = 1)]
public class BulletHellSequenceCreator : ScriptableSingleton<BulletHellSequenceCreator>
{
    [NonSerialized] public List<BulletEvent.SerializableBulletEventData> events;
}

#if UNITY_EDITOR
[CustomEditor(typeof(BulletHellSequenceCreator)), CanEditMultipleObjects]
public class BulletHellSequenceCreatorEditor : Editor
{
    private bool listToggle;
    private List<bool> elementToggles = new List<bool>();

    private string filePath = "Assets/Bullet Events";
    private string fileName;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BulletHellSequenceCreator manager = (BulletHellSequenceCreator)target;

        if (manager.events == null) manager.events = new List<BulletEvent.SerializableBulletEventData>();

        EditorGUILayout.BeginHorizontal();
        listToggle = EditorGUILayout.Foldout(listToggle, "Events", true);
        int size = Mathf.Max(0, EditorGUILayout.IntField(manager.events.Count));
        EditorGUILayout.EndHorizontal();

        while (size > manager.events.Count)
        {
            manager.events.Add(new BulletEvent.SerializableBulletEventData<BasicEvent>(0f, 0f, 0f, new BulletPattern.SerializableBulletPatternData<SinglePattern>(new(0f, 0f), BulletType.Straight, 0f)));
        }
        while (size < manager.events.Count)
        {
            manager.events.RemoveAt(manager.events.Count - 1);
        }

        if (listToggle)
        {
            while (manager.events.Count > elementToggles.Count)
            {
                elementToggles.Add(false);
            }
            while (manager.events.Count < elementToggles.Count)
            {
                elementToggles.RemoveAt(elementToggles.Count - 1);
            }

            for (int i = 0; i < manager.events.Count; i++)
            {
                elementToggles[i] = EditorGUILayout.Foldout(elementToggles[i], "Event " + i, true);

                if (elementToggles[i])
                {
                    manager.events[i] = DrawEvent(manager.events[i]);

                    manager.events[i] = DrawPattern(manager.events[i]);
                }
            }
        }

        EditorGUILayout.LabelField("File Path");
        filePath = EditorGUILayout.TextField(filePath);

        EditorGUILayout.LabelField("File Name");
        fileName = EditorGUILayout.TextField(fileName);

        EditorGUILayout.BeginHorizontal();
        if (EditorGUILayout.LinkButton("Save"))
        {
            string json = UnityNewtonsoftJsonSerializer.instance.Serialize(manager.events);
            File.WriteAllText(filePath + "/" + fileName + ".json", json);
            if (File.Exists(filePath + "/" + fileName + ".json"))
            {
                Debug.Log("Bullet Event Sequence Saved");
            }
        }
        if (EditorGUILayout.LinkButton("Load"))
        {
            if (File.Exists(filePath + "/" + fileName + ".json"))
            {
                string loadedFile = File.ReadAllText(filePath + "/" + fileName + ".json");
                manager.events = BulletEvent.DeserializeBulletEventArray(loadedFile);
            }
            else
            {
                Debug.LogWarning($"File {fileName} does not exist at {filePath}");
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private BulletEvent.SerializableBulletEventData DrawEvent(BulletEvent.SerializableBulletEventData bulletEvent)
    {
        EditorGUILayout.LabelField("Event Data");

        EventType eventType = bulletEvent.eventType;
        eventType = (EventType)EditorGUILayout.EnumPopup("Bullet Event Type", eventType);
        if (eventType != bulletEvent.eventType)
        {
            switch (eventType)
            {
                case EventType.Basic:
                    bulletEvent = new BulletEvent.SerializableBulletEventData<BasicEvent>(bulletEvent.startTime, bulletEvent.duration, bulletEvent.interval, bulletEvent.pattern);
                    break;

                case EventType.Spiral:
                    bulletEvent = new BulletEvent.SerializableBulletEventData<SpiralEvent>(bulletEvent.startTime, bulletEvent.duration, bulletEvent.interval, bulletEvent.pattern, 0f);
                    break;

                case EventType.Targeted:
                    bulletEvent = new BulletEvent.SerializableBulletEventData<TargetedEvent>(bulletEvent.startTime, bulletEvent.duration, bulletEvent.interval, bulletEvent.pattern);
                    break;

                case EventType.RandomDirection:
                    bulletEvent = new BulletEvent.SerializableBulletEventData<RandomDirectionEvent>(bulletEvent.startTime, bulletEvent.duration, bulletEvent.interval, bulletEvent.pattern, 0f, 0f);
                    break;

                case EventType.RandomPosition:
                    bulletEvent = new BulletEvent.SerializableBulletEventData<RandomPositionEvent>(bulletEvent.startTime, bulletEvent.duration, bulletEvent.interval, bulletEvent.pattern, 0f, 0f, RectTransform.Axis.Horizontal);
                    break;
            }
        }

        EditorGUILayout.LabelField("Timing");

        bulletEvent.startTime = EditorGUILayout.FloatField("Start", bulletEvent.startTime);
        bulletEvent.duration = EditorGUILayout.FloatField("Duration", bulletEvent.duration);
        bulletEvent.interval = EditorGUILayout.FloatField("Interval", bulletEvent.interval);

        switch (eventType)
        {
            case EventType.Basic:

                break;

            case EventType.Spiral:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                BulletEvent.SerializableBulletEventData<SpiralEvent> spiralEvent = (BulletEvent.SerializableBulletEventData<SpiralEvent>)bulletEvent;
                spiralEvent.additionalParams[0] = new KeyValuePair<string, object>(spiralEvent.additionalParams[0].Key, EditorGUILayout.FloatField("Rotation Speed", Convert.ToSingle(spiralEvent.additionalParams[0].Value)));

                break;

            case EventType.Targeted:

                break;

            case EventType.RandomDirection:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                BulletEvent.SerializableBulletEventData<RandomDirectionEvent> randomDirectionEvent = (BulletEvent.SerializableBulletEventData<RandomDirectionEvent>)bulletEvent;

                randomDirectionEvent.additionalParams[0] = new KeyValuePair<string, object>(randomDirectionEvent.additionalParams[0].Key, EditorGUILayout.FloatField("Minimum Angle", Convert.ToSingle(randomDirectionEvent.additionalParams[0].Value)));
                randomDirectionEvent.additionalParams[1] = new KeyValuePair<string, object>(randomDirectionEvent.additionalParams[1].Key, EditorGUILayout.FloatField("Maximum Angle", Convert.ToSingle(randomDirectionEvent.additionalParams[1].Value)));
                break;

            case EventType.RandomPosition:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                BulletEvent.SerializableBulletEventData<RandomPositionEvent> randomPositionEvent = (BulletEvent.SerializableBulletEventData<RandomPositionEvent>)bulletEvent;

                randomPositionEvent.additionalParams[0] = new KeyValuePair<string, object>(randomPositionEvent.additionalParams[0].Key, EditorGUILayout.FloatField("Minimum Distance", Convert.ToSingle(randomPositionEvent.additionalParams[0].Value)));
                randomPositionEvent.additionalParams[1] = new KeyValuePair<string, object>(randomPositionEvent.additionalParams[1].Key, EditorGUILayout.FloatField("Maximum Distance", Convert.ToSingle(randomPositionEvent.additionalParams[1].Value)));
                randomPositionEvent.additionalParams[2] = new KeyValuePair<string, object>(randomPositionEvent.additionalParams[2].Key, (RectTransform.Axis)EditorGUILayout.EnumPopup("Axis", (RectTransform.Axis)Convert.ToInt32(randomPositionEvent.additionalParams[2].Value)));
                break;
        }

        EditorGUILayout.Space();

        return bulletEvent;
    }

    private BulletEvent.SerializableBulletEventData DrawPattern(BulletEvent.SerializableBulletEventData bulletEvent)
    {
        EditorGUILayout.LabelField("Pattern Data");

        PatternType patternType = bulletEvent.pattern.patternType;
        patternType = (PatternType)EditorGUILayout.EnumPopup("Bullet Event Type", patternType);
        if (patternType != bulletEvent.pattern.patternType)
        {
            switch (patternType)
            {
                case PatternType.Single:
                    bulletEvent.pattern = new BulletPattern.SerializableBulletPatternData<SinglePattern>(bulletEvent.pattern.spawnPoint, bulletEvent.pattern.bulletType, bulletEvent.pattern.direction);
                    break;

                case PatternType.Ring:
                    bulletEvent.pattern = new BulletPattern.SerializableBulletPatternData<RingPattern>(bulletEvent.pattern.spawnPoint, bulletEvent.pattern.bulletType, bulletEvent.pattern.direction, 0);
                    break;

                case PatternType.RingWithGap:
                    bulletEvent.pattern = new BulletPattern.SerializableBulletPatternData<RingWithGapPattern>(bulletEvent.pattern.spawnPoint, bulletEvent.pattern.bulletType, bulletEvent.pattern.direction, 0f, 0);
                    break;

                case PatternType.Line:
                    bulletEvent.pattern = new BulletPattern.SerializableBulletPatternData<LinePattern>(bulletEvent.pattern.spawnPoint, bulletEvent.pattern.bulletType, bulletEvent.pattern.direction, 0, 0f);
                    break;

                case PatternType.LineWithGap:
                    bulletEvent.pattern = new BulletPattern.SerializableBulletPatternData<LineWithGapPattern>(bulletEvent.pattern.spawnPoint, bulletEvent.pattern.bulletType, bulletEvent.pattern.direction, 0f, 0, 0f);
                    break;
            }
        }

        bulletEvent.pattern.spawnPoint = EditorGUILayout.Vector2Field("Spawn Point", bulletEvent.pattern.spawnPoint);
        bulletEvent.pattern.bulletType = (BulletType)EditorGUILayout.EnumPopup("Bullet Movement Type", bulletEvent.pattern.bulletType);
        bulletEvent.pattern.direction = EditorGUILayout.FloatField("Direction", bulletEvent.pattern.direction);

        switch (patternType)
        {
            case PatternType.Single:

                break;

            case PatternType.Ring:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                BulletPattern.SerializableBulletPatternData<RingPattern> ringPattern = (BulletPattern.SerializableBulletPatternData<RingPattern>)bulletEvent.pattern;

                ringPattern.additionalParams[0] = new KeyValuePair<string, object>(ringPattern.additionalParams[0].Key, EditorGUILayout.IntField("Ring Density", Convert.ToInt32(ringPattern.additionalParams[0].Value)));
                break;

            case PatternType.RingWithGap:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                BulletPattern.SerializableBulletPatternData<RingWithGapPattern> ringWithGapPattern = (BulletPattern.SerializableBulletPatternData<RingWithGapPattern>)bulletEvent.pattern;

                ringWithGapPattern.additionalParams[1] = new KeyValuePair<string, object>(ringWithGapPattern.additionalParams[1].Key, EditorGUILayout.IntField("Ring Density", Convert.ToInt32(ringWithGapPattern.additionalParams[1].Value)));
                ringWithGapPattern.additionalParams[0] = new KeyValuePair<string, object>(ringWithGapPattern.additionalParams[0].Key, EditorGUILayout.FloatField("Gap Size", Convert.ToSingle(ringWithGapPattern.additionalParams[0].Value)));
                break;

            case PatternType.Line:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                BulletPattern.SerializableBulletPatternData<LinePattern> linePattern = (BulletPattern.SerializableBulletPatternData<LinePattern>)bulletEvent.pattern;

                linePattern.additionalParams[0] = new KeyValuePair<string, object>(linePattern.additionalParams[0].Key, EditorGUILayout.IntField("Line Density", Convert.ToInt32(linePattern.additionalParams[0].Value)));
                linePattern.additionalParams[1] = new KeyValuePair<string, object>(linePattern.additionalParams[1].Key, EditorGUILayout.FloatField("Line Length", Convert.ToSingle(linePattern.additionalParams[1].Value)));
                break;

            case PatternType.LineWithGap:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                BulletPattern.SerializableBulletPatternData<LineWithGapPattern> lineWithGapPattern = (BulletPattern.SerializableBulletPatternData<LineWithGapPattern>)bulletEvent.pattern;

                lineWithGapPattern.additionalParams[1] = new KeyValuePair<string, object>(lineWithGapPattern.additionalParams[1].Key, EditorGUILayout.IntField("Line Density", Convert.ToInt32(lineWithGapPattern.additionalParams[1].Value)));
                lineWithGapPattern.additionalParams[2] = new KeyValuePair<string, object>(lineWithGapPattern.additionalParams[2].Key, EditorGUILayout.FloatField("Line Length", Convert.ToSingle(lineWithGapPattern.additionalParams[2].Value)));
                lineWithGapPattern.additionalParams[0] = new KeyValuePair<string, object>(lineWithGapPattern.additionalParams[0].Key, EditorGUILayout.FloatField("Gap Size", Convert.ToSingle(lineWithGapPattern.additionalParams[0].Value)));
                break;
        }

        EditorGUILayout.Space();

        return bulletEvent;
    }
}
#endif