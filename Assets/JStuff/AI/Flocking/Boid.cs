using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{

    public class Boid : MonoBehaviour
    {
        public Vector2 velocity = new Vector2(0,0);

        public void UpdatePosition()
        {
            Vector2 v = this.transform.position;
            v += velocity;
            this.transform.position = v;
        }
    }
}