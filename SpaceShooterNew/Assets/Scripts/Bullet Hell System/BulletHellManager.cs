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
    private BulletMethod selectedMethod;

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
        selectedMethod = new BasicMethod(0f, 10f, 2f, selectedPattern);
    }

    public void RunPattern()
    {
        StartCoroutine(selectedMethod.Run());
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

        selectedMethod.SetPattern(selectedPattern);
    }

    public void OnMethodChanged(int value)
    {
        switch (value)
        {
            case 0:
                selectedMethod = new BasicMethod(0f, 10f, 2f, selectedPattern);
                break;

            case 1:
                selectedMethod = new SpiralMethod(0f, 10f, 2f, selectedPattern, density);
                break;

            case 2:
                selectedMethod = new TargetedMethod(0f, 10f, 2f, selectedPattern, target);
                break;

            case 3:
                selectedMethod = new RandomDirectionMethod(0f, 10f, 2f, selectedPattern, -45f, 45f);
                break;

            case 4:
                selectedMethod = new RandomPositionMethod(0f, 10f, 2f, selectedPattern, -3f, 3f, RectTransform.Axis.Horizontal);
                break;

            default:
                selectedMethod = new BasicMethod(0f, 10f, 2f, selectedPattern);
                break;
        }
    }
}
