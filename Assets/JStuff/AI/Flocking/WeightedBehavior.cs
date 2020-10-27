using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace JStuff.AI.Flocking
{
    [Serializable]
    public class WeightedBehavior
    {
        [SerializeField] private FlockingBehavior behavior;
        [SerializeField] private float weight;
        [SerializeField] private List<Transform> context;

        public float Weight
        {
            get { return weight; }
        }

        public FlockingBehavior Behavior
        {
            get { return behavior; }
        }

        public List<Transform> Context
        {
            get { return context; }
        }

        public void SetContext(List<Transform> context)
        {
            this.context = context;
        }
    }
}