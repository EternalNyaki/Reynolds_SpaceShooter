using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

[PreferBinarySerialization, CreateAssetMenu(fileName = "Bullet Hell Sequence", menuName = "ScriptableObjects/BulletHellSequence", order = 1)]
public class BulletHellSequence : ScriptableObject
{
    public List<BulletEvent> events = new List<BulletEvent>();
}

#if UNITY_EDITOR
[CustomEditor(typeof(BulletHellSequence)), CanEditMultipleObjects]
public class BulletHellSequenceEditor : Editor
{
    private bool listToggle;
    private List<bool> elementToggles = new List<bool>();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BulletHellSequence manager = (BulletHellSequence)target;

        EditorGUILayout.BeginHorizontal();
        listToggle = EditorGUILayout.Foldout(listToggle, "Events", true);
        int size = Mathf.Max(0, EditorGUILayout.IntField(manager.events.Count));
        EditorGUILayout.EndHorizontal();

        while (size > manager.events.Count)
        {
            manager.events.Add(new BasicEvent(0f, 0f, 0f, new SinglePattern(null, null, 0f)));
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
    }

    private BulletEvent DrawEvent(BulletEvent bulletEvent)
    {
        EditorGUILayout.LabelField("Event Data");

        ref EventType eventType = ref bulletEvent.GetType().GetCustomAttribute<EventTypeAttribute>().type;
        eventType = (EventType)EditorGUILayout.EnumPopup("Bullet Event Type", eventType);
        if (eventType != bulletEvent.GetType().GetCustomAttribute<EventTypeAttribute>().type)
        {
            switch (eventType)
            {
                case EventType.Basic:
                    bulletEvent = BulletEvent.CloneBaseValues(bulletEvent, new BasicEvent());
                    break;

                case EventType.Spiral:
                    bulletEvent = BulletEvent.CloneBaseValues(bulletEvent, new SpiralEvent());
                    break;

                case EventType.Targeted:
                    bulletEvent = BulletEvent.CloneBaseValues(bulletEvent, new TargetedEvent());
                    break;

                case EventType.RandomDirection:
                    bulletEvent = BulletEvent.CloneBaseValues(bulletEvent, new RandomDirectionEvent());
                    break;

                case EventType.RandomPosition:
                    bulletEvent = BulletEvent.CloneBaseValues(bulletEvent, new RandomPositionEvent());
                    break;
            }
        }

        EditorGUILayout.LabelField("Timing");

        bulletEvent.startTime = EditorGUILayout.FloatField("Start", bulletEvent.startTime);
        bulletEvent.duration = EditorGUILayout.FloatField("Duration", bulletEvent.duration);

        switch (eventType)
        {
            case EventType.Basic:

                break;

            case EventType.Spiral:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                SpiralEvent spiralEvent = (SpiralEvent)bulletEvent;

                spiralEvent.deltaAngle = EditorGUILayout.FloatField("Rotation Speed", spiralEvent.deltaAngle);

                break;

            case EventType.Targeted:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                TargetedEvent targetedEvent = (TargetedEvent)bulletEvent;

                targetedEvent.target = (Transform)EditorGUILayout.ObjectField("Target", targetedEvent.target, typeof(Transform), true);
                break;

            case EventType.RandomDirection:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                RandomDirectionEvent randomDirectionEvent = (RandomDirectionEvent)bulletEvent;

                randomDirectionEvent.minAngle = EditorGUILayout.FloatField("Minimum Angle", randomDirectionEvent.minAngle);
                randomDirectionEvent.maxAngle = EditorGUILayout.FloatField("Maximum Angle", randomDirectionEvent.maxAngle);
                break;

            case EventType.RandomPosition:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                RandomPositionEvent randomPositionEvent = (RandomPositionEvent)bulletEvent;

                randomPositionEvent.minDistance = EditorGUILayout.FloatField("Minimum Angle", randomPositionEvent.minDistance);
                randomPositionEvent.maxDistance = EditorGUILayout.FloatField("Maximum Angle", randomPositionEvent.maxDistance);
                randomPositionEvent.axis = (RectTransform.Axis)EditorGUILayout.EnumPopup("Axis", randomPositionEvent.axis);
                break;
        }

        EditorGUILayout.Space();

        return bulletEvent;
    }

    private BulletEvent DrawPattern(BulletEvent bulletEvent)
    {
        EditorGUILayout.LabelField("Pattern Data");

        PatternType patternType = bulletEvent.pattern.GetType().GetCustomAttribute<PatternTypeAttribute>().type;
        patternType = (PatternType)EditorGUILayout.EnumPopup("Bullet Event Type", patternType);
        if (patternType != bulletEvent.pattern.GetType().GetCustomAttribute<PatternTypeAttribute>().type)
        {
            switch (patternType)
            {
                case PatternType.Single:
                    bulletEvent.pattern = new SinglePattern();
                    break;

                case PatternType.Ring:
                    bulletEvent.pattern = new RingPattern();
                    break;

                case PatternType.RingWithGap:
                    bulletEvent.pattern = new RingWithGapPattern();
                    break;

                case PatternType.Line:
                    bulletEvent.pattern = new LinePattern();
                    break;

                case PatternType.LineWithGap:
                    bulletEvent.pattern = new LineWithGapPattern();
                    break;
            }
        }

        bulletEvent.pattern.offset = EditorGUILayout.Vector2Field("Spawn Point", bulletEvent.pattern.offset);
        bulletEvent.pattern.bulletPrefab = (GameObject)EditorGUILayout.ObjectField("Bullet Type", bulletEvent.pattern.bulletPrefab, typeof(GameObject), false);
        bulletEvent.pattern.direction = EditorGUILayout.FloatField("Direction", bulletEvent.pattern.direction);

        switch (bulletEvent.pattern.GetType().GetCustomAttribute<PatternTypeAttribute>().type)
        {
            case PatternType.Single:

                break;

            case PatternType.Ring:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                RingPattern ringPattern = (RingPattern)bulletEvent.pattern;

                ringPattern.density = EditorGUILayout.IntField("Ring Density", ringPattern.density);
                break;

            case PatternType.RingWithGap:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                RingWithGapPattern ringWithGapPattern = (RingWithGapPattern)bulletEvent.pattern;

                ringWithGapPattern.density = EditorGUILayout.IntField("Ring Density", ringWithGapPattern.density);
                ringWithGapPattern.gapSize = EditorGUILayout.FloatField("Gap Size", ringWithGapPattern.gapSize);
                break;

            case PatternType.Line:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                LinePattern linePattern = (LinePattern)bulletEvent.pattern;

                linePattern.length = EditorGUILayout.FloatField("Line Length", linePattern.length);
                linePattern.density = EditorGUILayout.IntField("Line Density", linePattern.density);
                break;

            case PatternType.LineWithGap:
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Extra Parameters");

                LineWithGapPattern lineWithGapPattern = (LineWithGapPattern)bulletEvent.pattern;

                lineWithGapPattern.length = EditorGUILayout.FloatField("Line Length", lineWithGapPattern.length);
                lineWithGapPattern.density = EditorGUILayout.IntField("Line Density", lineWithGapPattern.density);
                lineWithGapPattern.gapSize = EditorGUILayout.FloatField("Gap Size", lineWithGapPattern.gapSize);
                break;
        }

        EditorGUILayout.Space();

        return bulletEvent;
    }
}
#endif