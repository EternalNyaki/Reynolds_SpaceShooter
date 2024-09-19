using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed;

    private Vector2 target;

    public Transform player;

    private void Start()
    {
        target = new Vector2(player.position.x, player.position.y);
    }

    private void Update()
    {
        Vector2 toTarget = target - (Vector2)transform.position;
        Vector2 deltaPos = toTarget.normalized * speed * Time.deltaTime;

        if (deltaPos.magnitude >= toTarget.magnitude)
        {
            transform.position = target;
            target = new Vector2(player.position.x, player.position.y);
        }
        else
        {
            transform.position += (Vector3)deltaPos;
        }

        Debug.DrawLine(transform.position, target, Color.green);
    }
}
