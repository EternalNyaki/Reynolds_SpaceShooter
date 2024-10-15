using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovement : BulletMovement
{
    protected override void Move()
    {
        transform.position += -transform.up * speed * Time.deltaTime;
    }
}
