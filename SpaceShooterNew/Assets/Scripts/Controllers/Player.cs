using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 6f;

    void Start()
    {

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
    }

    private void PlayerMovement(Vector2 inputVector)
    {
        transform.position += (Vector3)inputVector.normalized * moveSpeed * Time.deltaTime;
    }
}
