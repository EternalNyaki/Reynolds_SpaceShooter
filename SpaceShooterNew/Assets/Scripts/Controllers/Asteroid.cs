using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed;
    public float arrivalDistance;
    public float maxFloatDistance;

    private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        ChooseTarget();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 toTarget = target - (Vector2)transform.position;
        Vector2 deltaPos = toTarget.normalized * moveSpeed;

        if (toTarget.magnitude < arrivalDistance)
        {
            ChooseTarget();
        }
        else
        {
            transform.position += (Vector3)deltaPos;
        }

        Debug.DrawLine(transform.position, target, Color.green);
    }

    private void ChooseTarget()
    {
        target = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0, maxFloatDistance) + (Vector2)transform.position;
    }
}
