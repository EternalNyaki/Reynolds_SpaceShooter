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
        Vector2 vectorToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
        int direction = (int)Mathf.Sign(angleToTarget - transform.rotation.z);
        float rotation = direction * rotationSpeed * Time.deltaTime;
        if (direction == 1 ? transform.rotation.z + rotation > angleToTarget : transform.rotation.z + rotation < angleToTarget)
        {
            rotation = angleToTarget - transform.rotation.z;
        }
        transform.Rotate(new(0, 0, rotation));

        base.Move();
    }
}
