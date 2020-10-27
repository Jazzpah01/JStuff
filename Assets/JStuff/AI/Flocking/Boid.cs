using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{

    public class Boid : MonoBehaviour
    {
        public Vector3 velocity = new Vector3();
        public void UpdatePosition()
        {
            //Vector2 v = this.transform.position;
            //v += velocity;
            //this.transform.position = v;
            this.transform.position += velocity * Time.deltaTime;
            this.transform.up = velocity;
        }
    }
}