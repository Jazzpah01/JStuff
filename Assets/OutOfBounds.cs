using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public float rangeX;
    public float rangeY;

    private void Update()
    {
        Vector2 v = transform.position;
        if (this.transform.position.x < -rangeX)
            v.x = rangeX;
        if (this.transform.position.x > rangeX)
            v.x = -rangeX;
        if (this.transform.position.y < -rangeY)
            v.y = rangeY;
        if (this.transform.position.y > rangeY)
            v.y = -rangeY;

        transform.position = v;
    }
}