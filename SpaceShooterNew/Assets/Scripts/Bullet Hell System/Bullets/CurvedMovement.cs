using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for having a bullet spiral outwards
public class CurvedMovement : StraightMovement
{
    //Rotation speed (in degrees/s)
    public float rotationSpeed;
    //Rotation decay (in degrees/s^2)
    public float spiralDecay;

    protected override void Move()
    {
        //Rotate by rotation speed
        transform.Rotate(new(0, 0, rotationSpeed * Time.deltaTime));
        base.Move();

        //Decrease rotation speed so the bullet spirals outwards
        rotationSpeed -= spiralDecay * Time.deltaTime;
    }
}
