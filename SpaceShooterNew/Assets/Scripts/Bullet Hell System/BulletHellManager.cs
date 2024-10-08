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

    // Start is called before the first frame update
    void Start()
    {
        single = new SinglePattern(transform, bulletPrefab, direction);
        ring = new RingPattern(transform, bulletPrefab, direction, density);
        ringWithGap = new RingWithGapPattern(transform, bulletPrefab, direction, density, ringGapSize);
        line = new LinePattern(transform, bulletPrefab, direction, density, length);
        lineWithGap = new LineWithGapPattern(transform, bulletPrefab, direction, density, length, gapPosition, lineGapSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            single.Spawn();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ring.Spawn();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ringWithGap.Spawn();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            line.Spawn();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            lineWithGap.Spawn();
        }
    }
}
