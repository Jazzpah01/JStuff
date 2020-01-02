using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{
    [CreateAssetMenu(menuName = "Assets/Flock/Behavior/Direction")]
    public class Direction : FlockingBehavior
    {
        public override Vector2 VelocityChange(Boid[] flock, Transform[] context, Boid boid)
        {
            return boid.velocity.normalized;
        }
    }
}