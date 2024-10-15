using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletMovement : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        //StartCoroutine(DeathTimer());
        Initialize();
    }

    protected virtual void Initialize()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected virtual void Move()
    {

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // private IEnumerator DeathTimer()
    // {
    //     yield return new WaitForSeconds(10f);
    //     Destroy(gameObject);
    // }
}
