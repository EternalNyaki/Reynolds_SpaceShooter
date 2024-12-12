using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public BulletHellSequence sequence;

    private BulletEvent[] _events;

    // Start is called before the first frame update
    void Start()
    {
        foreach (BulletEvent bEvent in _events) StartCoroutine(bEvent.Run());
    }

    // Update is called once per frame
    void Update()
    {

    }
}