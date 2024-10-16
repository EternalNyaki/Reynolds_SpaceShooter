using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for having a bullet follow a set path
public class PathedMovement : BulletMovement
{
    //Path for bullet to follow ((0, 0) is the bullet's spawn point)
    public List<Vector2> path;

    protected override void Initialize()
    {
        //Rotate path to match transform rotation
        for (int i = 0; i < path.Count; i++)
        {
            path[i] = RotateVectorByAngle(transform.rotation.eulerAngles.z * Mathf.Deg2Rad, path[i]);
        }

        StartCoroutine(FollowPath());
    }

    //Coroutine to follow path
    private IEnumerator FollowPath()
    {
        //Set starting position
        Vector2 startPosition = (Vector2)transform.position;

        while (path.Count > 0)
        {
            //Lazy function for getting vector to the next point in the path
            Func<Vector2> toPoint = () => { return path[0] + startPosition - (Vector2)transform.position; };
            while (toPoint().magnitude - speed * Time.deltaTime > 0)
            {
                //Move towards the next point in the path 
                transform.position += (Vector3)toPoint().normalized * speed * Time.deltaTime;
                yield return null;
            }

            //Remove the first point in the path once the next movement would put the bullet past it
            path.RemoveAt(0);
        }

        //Destroy the bullet once it's reached the end of the path
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
