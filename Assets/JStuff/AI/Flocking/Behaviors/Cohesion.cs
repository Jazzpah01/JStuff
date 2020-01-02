using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{
    [CreateAssetMenu(menuName = "Assets/Flock/Behavior/Cohesion")]
    public class Cohesion : FlockingBehavior
    {
        public float range;
        public override Vector2 VelocityChange(Boid[] flock, Transform[] context, Boid boid)
        {
            Vector2 v = new Vector2(0, 0);
            Boid[] bb = Flock.BoidsInRadius(flock, boid.transform.position, range);
            foreach (Boid b in bb)
            {
                Vector2 v2 = b.transform.position;
                v += v2;
            }
            v /= bb.Length;
            Vector2 v3 = boid.transform.position;
            v -= v3;
            return (v.magnitude > 0) ? v.normalized : new Vector2(0, 0);
        }
    }
}