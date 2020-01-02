using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace JStuff.AI.Flocking
{
    [Serializable]
    public class WeightedBehavior
    {
        [SerializeField]
        private FlockingBehavior behavior;
        [SerializeField]
        private float weight;
        [SerializeField]
        //private List<Transform> context;
        private Transform[] context;

        public float Weight
        {
            get { return weight; }
        }

        public FlockingBehavior Behavior
        {
            get { return behavior; }
        }

        public Transform[] Context
        {
            get { return context; }
        }
    }
}