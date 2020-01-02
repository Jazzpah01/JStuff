﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JStuff.AI.Flocking
{
    [CreateAssetMenu(menuName = "Assets/Flock/Behavior/Random")]
    public class RandomUrge : FlockingBehavior
    {
        public override Vector2 VelocityChange(Boid[] flock, Transform[] context, Boid boid)
        {
            float random = UnityEngine.Random.Range(0f, 260f);
            return new Vector2(Mathf.Cos(random), Mathf.Sin(random)).normalized;
        }
    }
}