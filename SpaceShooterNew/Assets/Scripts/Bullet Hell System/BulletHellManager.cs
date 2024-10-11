using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float direction;
    public int density;
    public float ringGapSize;
    public float length;
    public float gapPosition;
    public float lineGapSize;

    private SinglePattern single;
    private RingPattern ring;
    private RingWithGapPattern ringWithGap;
    private LinePattern line;
    private LineWithGapPattern lineWithGap;

    private BulletPattern selectedPattern;
    private BulletEvent selectedEvent;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        single = new SinglePattern(transform, bulletPrefab, direction);
        ring = new RingPattern(transform, bulletPrefab, direction, density);
        ringWithGap = new RingWithGapPattern(transform, bulletPrefab, direction, density * 5, ringGapSize);
        line = new LinePattern(transform, bulletPrefab, direction, density, length);
        lineWithGap = new LineWithGapPattern(transform, bulletPrefab, direction, density * 5, length, gapPosition, lineGapSize);

        selectedPattern = single;
        selectedEvent = new BasicEvent(0f, 10f, 2f, selectedPattern);
    }

    public void RunPattern()
    {
        StartCoroutine(selectedEvent.Run());
    }

    public void OnPatternChanged(int value)
    {
        switch (value)
        {
            case 0:
                selectedPattern = single;
                break;

            case 1:
                selectedPattern = ring;
                break;

            case 2:
                selectedPattern = ringWithGap;
                break;

            case 3:
                selectedPattern = line;
                break;

            case 4:
                selectedPattern = lineWithGap;
                break;

            default:
                selectedPattern = single;
                break;
        }

        selectedEvent.SetPattern(selectedPattern);
    }

    public void OnEventChanged(int value)
    {
        switch (value)
        {
            case 0:
                selectedEvent = new BasicEvent(0f, 10f, 2f, selectedPattern);
                break;

            case 1:
                selectedEvent = new SpiralEvent(0f, 10f, 2f, selectedPattern, density);
                break;

            case 2:
                selectedEvent = new TargetedEvent(0f, 10f, 2f, selectedPattern, target);
                break;

            case 3:
                selectedEvent = new RandomDirectionEvent(0f, 10f, 2f, selectedPattern, -45f, 45f);
                break;

            case 4:
                selectedEvent = new RandomPositionEvent(0f, 10f, 2f, selectedPattern, -3f, 3f, RectTransform.Axis.Horizontal);
                break;

            case 5:
                TargetedEvent targetedEvent = new TargetedEvent(0f, 10f, 2f, selectedPattern, target);
                selectedEvent = BulletEvent.CombineEvents(targetedEvent, BulletEvent.CloneBaseValues(targetedEvent, new RandomPositionEvent(0f, 0f, 0f, null, -3f, 3f, RectTransform.Axis.Horizontal)));
                break;

            default:
                selectedEvent = new BasicEvent(0f, 10f, 2f, selectedPattern);
                break;
        }
    }
}
