using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime;

    void Start()
    {
        StartCoroutine(DrawStars());
    }

    private IEnumerator DrawStars()
    {
        for (int i = 0; i < starTransforms.Count - 1; i++)
        {
            float timer = 0;
            while (timer < drawingTime)
            {
                for (int j = 0; j < i; j++)
                {
                    Debug.DrawLine(starTransforms[j].position, starTransforms[j + 1].position, Color.white);
                }
                Debug.DrawLine(starTransforms[i].position, Vector3.Lerp(starTransforms[i].position, starTransforms[i + 1].position, timer / drawingTime), Color.white);

                timer += Time.deltaTime;

                yield return null;
            }
        }
    }
}
