using JStuff.AI.Flocking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockTest : MonoBehaviour
{
    public Flock flock;
    public int behaviorindex;

    // Start is called before the first frame update
    void Start()
    {
        List<Transform> context = new List<Transform>();
        foreach (Boid b in flock.GetComponentsInChildren<Boid>())
        {
            flock.AddContext(behaviorindex, b.transform);
        }
    }
}