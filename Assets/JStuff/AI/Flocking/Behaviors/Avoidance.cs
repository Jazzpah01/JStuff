using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{
    [CreateAssetMenu(menuName = "Assets/Flock/Behavior/Avoidance")]
    public class Avoidance : FlockingBehavior
    {
        public float range;
        public override Vector2 VelocityChange(Boid[] flock, Transform[] context, Boid boid)
        {
            Vector2 retval = Vector2.zero;
            foreach (Transform t in context)
            {
                Vector2 delta = t.position - boid.transform.position;
                if ((delta).magnitude < range)
                {
                    retval -= delta.normalized;
                }
            }
            return retval.normalized;
        }
    }
}