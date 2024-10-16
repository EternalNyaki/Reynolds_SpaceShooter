using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for bullet movement scripts
public abstract class BulletMovement : MonoBehaviour
{
    //Movement speed (in units/s)
    public float speed;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Initialization code goes here
    /// Called in Start()
    /// </summary>
    protected virtual void Initialize()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// Movement code goes here
    /// Called every Update()
    /// </summary>
    protected virtual void Move()
    {

    }

    void OnBecameInvisible()
    {
        //Destroy bullet when it leaves the screen
        Destroy(gameObject);
    }
}
