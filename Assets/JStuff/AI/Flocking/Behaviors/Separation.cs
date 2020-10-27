using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{
    [CreateAssetMenu(menuName = "Assets/Flock/Behavior/Separation")]
    public class Separation : FlockingBehavior
    {
        public float distance;
        public override Vector2 VelocityChange(Flock flock, Boid boid, List<Transform> context)
        {
            (Boid b, float dis) = flock.NearestBoid(boid);
            Vector2 v = b.transform.position - boid.transform.position;
            float r = 1 - 2 * distance / (v.magnitude + distance);
            return (dis != 0) ? r * v.normalized : r * new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        }
    }
}