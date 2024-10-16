using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellManager : MonoBehaviour
{
    public float direction;
    public int density;
    public float ringGapSize;
    public float length;
    public float gapPosition;
    public float lineGapSize;
    public Transform target;
    public GameObject[] bulletPrefabs;

    private BulletPattern selectedPattern;
    private BulletEvent selectedEvent;
    private GameObject selectedBullet;

    // Start is called before the first frame update
    void Start()
    {
        selectedBullet = bulletPrefabs[0];
        selectedPattern = new SinglePattern(transform, selectedBullet, direction);
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
                selectedPattern = new SinglePattern(transform, selectedBullet, direction);
                break;

            case 1:
                selectedPattern = new RingPattern(transform, selectedBullet, direction, density);
                break;

            case 2:
                selectedPattern = new RingWithGapPattern(transform, selectedBullet, direction, density * 5, ringGapSize);
                break;

            case 3:
                selectedPattern = new LinePattern(transform, selectedBullet, direction, 1, length);
                break;

            case 4:
                selectedPattern = new LineWithGapPattern(transform, selectedBullet, direction, density, length, gapPosition, lineGapSize);
                break;

            default:
                selectedPattern = new SinglePattern(transform, selectedBullet, direction);
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
                selectedEvent = new RandomPositionEvent(0f, 10f, 2f, selectedPattern, -5f, 5f, RectTransform.Axis.Horizontal);
                break;

            case 5:
                RandomPositionEvent randomPositionEvent = new RandomPositionEvent(0f, 10f, 2f, selectedPattern, -5f, 5f, RectTransform.Axis.Horizontal);
                selectedEvent = BulletEvent.CombineEvents(randomPositionEvent, BulletEvent.CloneBaseValues(randomPositionEvent, new TargetedEvent(0f, 10f, 2f, selectedPattern, target)));
                break;

            default:
                selectedEvent = new BasicEvent(0f, 10f, 2f, selectedPattern);
                break;
        }
    }

    public void OnBulletTypeChanged(int value)
    {
        if (value < bulletPrefabs.Length)
        {
            selectedBullet = bulletPrefabs[value];
        }
        else
        {
            selectedBullet = bulletPrefabs[0];
        }
        selectedPattern.SetBulletType(selectedBullet);
    }
}
