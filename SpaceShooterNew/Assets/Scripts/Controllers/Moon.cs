using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public float orbitalRadius;
    public float orbitalSpeed;

    private float angle = 0;

    public Transform planetTransform;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * orbitalRadius + planetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        OrbitalMotion(orbitalRadius, orbitalSpeed, planetTransform);
    }

    private void OrbitalMotion(float radius, float speed, Transform target)
    {
        angle += speed * Time.deltaTime;
        angle %= 360;
        transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius + target.position;
    }
}
