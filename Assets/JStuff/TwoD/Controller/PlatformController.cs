using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JStuff;

public class PlatformController : MonoBehaviour
{
    public Body2d body;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            body.JumpInput(1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.HorizontalInput(-1);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.HorizontalInput(1);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            body.FallInput();
        }
    }
}