using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for moving a bullet straight at a constant speed
public class StraightMovement : BulletMovement
{
    protected override void Move()
    {
        //Move forward by speed
        transform.position += -transform.up * speed * Time.deltaTime;
    }
}
