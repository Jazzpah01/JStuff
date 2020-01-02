using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{
    [CreateAssetMenu(menuName = "Assets/Flock/Behavior/Alignment")]
    public class Alignment : FlockingBehavior
    {
        public float range;
        public override Vector2 VelocityChange(Boid[] flock, Transform[] context, Boid boid)
        {
            Vector2 v = Vector2.zero;
            Boid[] bb = Flock.BoidsInRadius(flock, boid.transform.position, range);
            foreach (Boid b in bb)
            {
                v += b.velocity;
            }
            return v / bb.Length;
        }
    }
}