using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed;
    public float accelerationTime;
    public float decelerationTime;

    public int radarCirclePoints;
    public float radarRadius;

    public int numberOfPowerups;
    public float powerupsRadius;

    private float acceleration;
    private float deceleration;
    private Vector2 velocity;

    public List<Transform> asteroidTransforms;
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public Transform bombsTransform;
    public GameObject powerupPrefab;

    void Start()
    {
        acceleration = maxSpeed / accelerationTime;
        deceleration = maxSpeed / decelerationTime;
    }

    void Update()
    {
        Vector2 input = new Vector2();

        //Fun alt code for getting input (~0.01ms/frame faster)
        // input.x = -Convert.ToByte(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) + Convert.ToByte(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D));
        // input.y = Convert.ToByte(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) - Convert.ToByte(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S));

        switch (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            case (true, false):
                input.x = -1;
                break;

            case (false, true):
                input.x = 1;
                break;

            default:
                input.x = 0;
                break;
        }
        switch (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            case (true, false):
                input.y = 1;
                break;

            case (false, true):
                input.y = -1;
                break;

            default:
                input.y = 0;
                break;
        }

        PlayerMovement(input.normalized);

        EnemyRadar(radarCirclePoints, radarRadius);

        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPowerups(numberOfPowerups, powerupsRadius);
        }
    }

    private void PlayerMovement(Vector2 inputVector)
    {
        float tempDecel = deceleration * Time.deltaTime;
        if (inputVector.x == 0)
        {
            if (tempDecel >= Mathf.Abs(velocity.x))
            {
                velocity.x = 0;
            }
            else
            {
                velocity.x -= Mathf.Sign(velocity.x) * tempDecel;
            }
        }
        else
        {
            velocity.x += inputVector.x * acceleration * Time.deltaTime;
        }

        if (inputVector.y == 0)
        {
            if (tempDecel >= Mathf.Abs(velocity.y))
            {
                velocity.y = 0;
            }
            else
            {
                velocity.y -= Mathf.Sign(velocity.y) * tempDecel;
            }
        }
        else
        {
            velocity.y += inputVector.y * acceleration * Time.deltaTime;
        }

        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        velocity.y = Mathf.Clamp(velocity.y, -maxSpeed, maxSpeed);

        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void EnemyRadar(int circlePoints, float radius)
    {
        Color lineColor = (transform.position - enemyTransform.position).magnitude < radius ? Color.red : Color.green;

        Vector2[] points = new Vector2[circlePoints];
        for (int i = 0; i < circlePoints; i++)
        {
            float theta = 360 / circlePoints * i;
            points[i] = new Vector2(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad)) * radius + (Vector2)transform.position;

            if (i > 0)
            {
                float distanceToEnemy = (transform.position - enemyTransform.position).magnitude;
                Debug.DrawLine(points[i - 1], points[i], lineColor);
            }
        }
        Debug.DrawLine(points[circlePoints - 1], points[0], lineColor);
    }

    private void SpawnPowerups(int numberOfPowerups, float radius)
    {
        for (int i = 0; i < numberOfPowerups; i++)
        {
            float theta = 360 / numberOfPowerups * i;
            Instantiate(powerupPrefab, new Vector3(Mathf.Cos(theta * Mathf.Deg2Rad), Mathf.Sin(theta * Mathf.Deg2Rad)) * radius + transform.position, Quaternion.identity);
        }
    }
}
