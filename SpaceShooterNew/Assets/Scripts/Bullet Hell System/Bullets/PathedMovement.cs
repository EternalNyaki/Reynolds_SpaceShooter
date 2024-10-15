using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathedMovement : BulletMovement
{
    public List<Vector2> path;

    protected override void Initialize()
    {
        //Rotate path to match transform rotation
        for (int i = 0; i < path.Count; i++)
        {
            path[i] = RotateVectorByAngle(transform.rotation.eulerAngles.z, path[i]);
        }

        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        Vector2 startPosition = (Vector2)transform.position;
        while (path.Count > 0)
        {
            Func<Vector2> toPoint = () => { return path[0] + startPosition - (Vector2)transform.position; };
            while (toPoint().magnitude - speed * Time.deltaTime > 0)
            {
                transform.position += (Vector3)toPoint().normalized * speed * Time.deltaTime;
                yield return null;
            }

            path.RemoveAt(0);
        }
        Destroy(gameObject);
    }

    //Made using this video by Freya Holmér: https://www.youtube.com/watch?v=7j5yW5QDC2U
    //Angle is in radians
    private Vector2 RotateVectorByAngle(float angle, Vector2 vector)
    {
        //Create local coordinate space
        Vector2 a = new(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 b = new(-Mathf.Sin(angle), Mathf.Cos(angle));

        //Multiple vector by coordinate space
        Vector2 output = vector.x * a + vector.y * b;
        return output;
    }
}
