using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public TextAsset bulletEventSequence;
    public Transform target;
    public GameObject bulletPrefab;

    private BulletEvent[] _events;

    // Start is called before the first frame update
    void Start()
    {
        var eventSequenceData = BulletEvent.DeserializeBulletEventArray(bulletEventSequence.text);
        List<BulletEvent> temp = new List<BulletEvent>();
        eventSequenceData.ForEach(e => temp.Add(BulletEvent.FromEventData(e, transform, target, bulletPrefab)));
        _events = temp.ToArray();

        foreach (BulletEvent e in _events) StartCoroutine(e.Run());
    }
}