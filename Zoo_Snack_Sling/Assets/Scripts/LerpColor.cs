using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    public Material material;
    public Color[] colors;
    private int current = 0;
    private int target = 1;
    private float targetPoint;
    public float time = 2.0f;

    // Update is called once per frame
    void Update()
    {
        TransitionColor();
    }

    void TransitionColor()
    {
        targetPoint += Time.deltaTime / time;
        material.color = Color.Lerp(colors[current], colors[target], targetPoint);
        if (targetPoint >= 1)
        {
            targetPoint = 0;
            current = target;
            target++;
            if (target == colors.Length)
            {
                target = 0;
            }
        }
    }
}
