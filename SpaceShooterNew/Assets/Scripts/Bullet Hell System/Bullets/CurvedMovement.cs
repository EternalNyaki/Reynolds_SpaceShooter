using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedMovement : StraightMovement
{
    public float rotationSpeed;
    public float spiralDecay;

    protected override void Move()
    {
        transform.Rotate(new(0, 0, rotationSpeed * Time.deltaTime));
        base.Move();

        rotationSpeed -= spiralDecay * Time.deltaTime;
    }
}
