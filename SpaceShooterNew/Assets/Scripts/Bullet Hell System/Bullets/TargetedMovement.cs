using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedMovement : StraightMovement
{
    public float rotationSpeed;
    public string targetTag;

    private Transform target;

    protected override void Initialize()
    {
        base.Initialize();
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }


    protected override void Move()
    {
        Vector2 vectorToTarget = target.position - transform.position;
        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
        float rotation;
        if (angleToTarget > transform.rotation.z)
        {
            rotation = rotationSpeed * Time.deltaTime;
            if (transform.rotation.z + rotation > angleToTarget)
            {
                rotation = angleToTarget - transform.rotation.z;
            }
        }
        else
        {
            rotation = -rotationSpeed * Time.deltaTime;
            if (transform.rotation.z + angleToTarget < angleToTarget)
            {
                rotation = angleToTarget - transform.rotation.z;
            }
        }
        transform.Rotate(new(0, 0, rotation));
        base.Move();
    }
}
