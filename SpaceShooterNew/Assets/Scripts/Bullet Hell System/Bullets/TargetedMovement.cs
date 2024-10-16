using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for having a bullet home in on a target
public class TargetedMovement : StraightMovement
{
    //Rotation speed (in degrees/s)
    public float rotationSpeed;
    //Tag of the Transform to target
    public string targetTag;

    //Transform of the target
    private Transform target;

    protected override void Initialize()
    {
        base.Initialize();

        //Get transform of target
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }


    protected override void Move()
    {
        //TODO: Make this less expensive with a large number of bullets
        //Get angle to target
        Vector2 vectorToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
        int direction = (int)Mathf.Sign(angleToTarget - transform.rotation.z);

        //Calculate rotation
        float rotation = direction * rotationSpeed * Time.deltaTime;
        if (direction == 1 ? transform.rotation.z + rotation > angleToTarget : transform.rotation.z + rotation < angleToTarget)
        {
            rotation = angleToTarget - transform.rotation.z;
        }

        //Rotate by calculated rotation
        transform.Rotate(new(0, 0, rotation));

        base.Move();
    }
}
