using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{
    [CreateAssetMenu(menuName = "Assets/Flock/Behavior/Cohesion")]
    public class Cohesion : FlockingBehavior
    {
        public float range;
        public override Vector2 VelocityChange(Flock flock, Boid boid, List<Transform> context)
        {
            Vector2 v = new Vector2(0, 0);
            List<Boid> bb = flock.BoidsInRadius(boid.transform.position, range);
            foreach (Boid b in bb)
            {
                Vector2 v2 = b.transform.position;
                v += v2;
            }
            v /= bb.Count;
            Vector2 v3 = boid.transform.position;
            v -= v3;
            return (v.magnitude > 0) ? v.normalized : new Vector2(0, 0);
        }
    }
}