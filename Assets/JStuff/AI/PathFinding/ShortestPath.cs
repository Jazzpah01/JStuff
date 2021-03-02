using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JStuff.Utilities;

public static class ShortestPath
{
    public delegate void SetPath(Vertex[] path);
    public delegate float Heuristic(Vertex v, Vertex u, object data = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="G">Graph.</param>
    /// <param name="s">Starting vertex.</param>
    /// <param name="t">Endind vertex/goal.</param>
    /// <returns>Returns array of shortest path from s to t INVERTED (t, ... , s).</returns>
    public static Vertex[] AStar(Graph G, Vertex s, Vertex t)
    {
        // Distance dist(v) to u is g(s->v) + h(v->t)
        // where g is the current shortest distance and h is a heuristic function mapping
        // an approximate distance between some vertex u and t. s is the starting vertex, 
        // u is some vertex, t is the goal vertex.

        Dictionary<Vertex, float> dist = new Dictionary<Vertex, float>();
        Dictionary<Vertex, float> rdist = new Dictionary<Vertex, float>();
        Dictionary<Vertex, Vertex> prev = new Dictionary<Vertex, Vertex>();

        foreach (Vertex v in G.GetVertices())
        {
            dist.Add(v, float.PositiveInfinity);
        }

        List<Vertex> S = new List<Vertex>();

        S.Add(s);
        rdist[s] = ManhattenDistance(s, t, 1);
        dist[s] = 0;

        int control = 1000;
        while (S.Count > 0 && control > 0)
        {
            control--;
            if (control < 1)
                throw new System.Exception("Infinite loop. :(");

            (List<Vertex> nS, Vertex v) = ExtractMinimum(S, rdist);
            S = nS;

            if (v.Equals(t))
                break;

            foreach (Vertex u in G.GetNeighbors(v))
            {
                if ((!prev.ContainsKey(u) && !u.Equals(s)) || 
                    (S.Contains(u) && dist[v] + G.GetWeight(v,u) < dist[u])) {
                    if (!S.Contains(u))
                        S.Add(u);
                    dist[u] = dist[v] + G.GetWeight(v, u);
                    rdist[u] = dist[u] + ManhattenDistance(u,t,1);
                    prev[u] = v;
                }
            }
        }
        

        if (prev.ContainsKey(t))
        {
            List<Vertex> route = new List<Vertex>();
            Vertex p = t;
            control = 1000;
            while (prev.ContainsKey(p))
            {
                route.Add(p);
                p = prev[p];
                control--;
                if (control < 1)
                    throw new System.Exception("Infinite loop. :(");
            }
            route.Add(s);
            return route.ToArray();
        } else
        {
            return null;
        }
    }

    public static Vertex[] AStar(Graph G, Vertex s, Vertex t, Heuristic h, HashSet<Vertex> excludedVertices = null, object hData = null)
    {
        // Distance dist(v) to u is g(s->v) + h(v->t)
        // where g is the current shortest distance and h is a heuristic function mapping
        // an approximate distance between some vertex u and t. s is the starting vertex, 
        // u is some vertex, t is the goal vertex.

        Dictionary<Vertex, float> dist = new Dictionary<Vertex, float>();
        Dictionary<Vertex, float> rdist = new Dictionary<Vertex, float>();
        Dictionary<Vertex, Vertex> prev = new Dictionary<Vertex, Vertex>();

        foreach (Vertex v in G.GetVertices())
        {
            dist.Add(v, float.PositiveInfinity);
        }

        List<Vertex> S = new List<Vertex>();

        S.Add(s);
        rdist[s] = h(s, t, hData);
        dist[s] = 0;

        int control = 1000;
        while (S.Count > 0 && control > 0)
        {
            control--;
            if (control < 1)
                throw new System.Exception("Infinite loop. :(");

            (List<Vertex> nS, Vertex v) = ExtractMinimum(S, rdist);
            S = nS;

            if (v.Equals(t))
                break;

            foreach (Vertex u in G.GetNeighbors(v))
            {
                if ((!prev.ContainsKey(u) && !u.Equals(s)) ||
                    (S.Contains(u) && dist[v] + G.GetWeight(v, u) < dist[u]))
                {
                    if (!S.Contains(u) && !excludedVertices.Contains(u))
                        S.Add(u);
                    dist[u] = dist[v] + G.GetWeight(v, u);
                    rdist[u] = dist[u] + h(u, t, hData);
                    prev[u] = v;
                }
            }
        }

        if (prev.ContainsKey(t))
        {
            List<Vertex> route = new List<Vertex>();
            Vertex p = t;
            control = 1000;
            while (prev.ContainsKey(p))
            {
                route.Add(p);
                p = prev[p];
                control--;
                if (control < 1)
                    throw new System.Exception("Infinite loop. :(");
            }
            route.Add(s);
            return route.ToArray();
        }
        else
        {
            return null;
        }
    }

    
    public static IEnumerator AStarCoroutine(Graph G, Vertex s, Vertex t, SetPath setPath, Heuristic h, HashSet<Vertex> excludedVertices = null, object hData = null)
    {
        // Distance dist(v) to u is g(s->v) + h(v->t)
        // where g is the current shortest distance and h is a heuristic function mapping
        // an approximate distance between some vertex u and t. s is the starting vertex, 
        // u is some vertex, t is the goal vertex.

        Debug.Log("AStarCoroutine");

        Dictionary<Vertex, float> dist = new Dictionary<Vertex, float>();
        Dictionary<Vertex, float> rdist = new Dictionary<Vertex, float>();
        Dictionary<Vertex, Vertex> prev = new Dictionary<Vertex, Vertex>();

        foreach (Vertex v in G.GetVertices())
        {
            dist.Add(v, float.PositiveInfinity);
        }

        List<Vertex> S = new List<Vertex>();

        S.Add(s);
        rdist[s] = h(s, t, hData);
        dist[s] = 0;

        int control = 1000;
        while (S.Count > 0 && control > 0)
        {
            control--;
            if (control < 1)
                throw new System.Exception("Infinite loop. :(");

            (List<Vertex> nS, Vertex v) = ExtractMinimum(S, rdist);
            S = nS;

            if (v.Equals(t))
                break;

            foreach (Vertex u in G.GetNeighbors(v))
            {
                if ((!prev.ContainsKey(u) && !u.Equals(s)) ||
                    (S.Contains(u) && dist[v] + G.GetWeight(v, u) < dist[u]))
                {
                    if (!S.Contains(u) && !excludedVertices.Contains(u))
                        S.Add(u);
                    dist[u] = dist[v] + G.GetWeight(v, u);
                    rdist[u] = dist[u] + h(s, t, hData);
                    prev[u] = v;
                }
            }

            yield return null;
        }

        if (prev.ContainsKey(t))
        {
            List<Vertex> route = new List<Vertex>();
            Vertex p = t;
            control = 1000;
            while (prev.ContainsKey(p))
            {
                route.Add(p);
                p = prev[p];
                control--;
                if (control < 1)
                    throw new System.Exception("Infinite loop. :(");
            }
            route.Add(s);
            setPath(route.ToArray());
            yield return null;
        }
        else
        {
            setPath(null);
            yield return null;
        }
    }

    /// <summary>
    /// Now with side effects :)
    /// </summary>
    /// <param name="S"></param>
    /// <param name="dist"></param>
    /// <returns></returns>
    private static (List<Vertex>, Vertex) ExtractMinimum(List<Vertex> S, Dictionary<Vertex, float> dist)
    {
        Vertex smallest = S[0];
        float d = dist[smallest];
        int control = 1000;
        foreach (Vertex v in S)
        {
            if (dist[v] < d)
            {
                d = dist[v];
                smallest = v;
            }
            control--;
            if (control < 1)
                throw new System.Exception("Infinite loop. :(");
        }
        S.Remove(smallest);
        return (S, smallest);
    }

    // http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <param name="u"></param>
    /// <param name="data">The smallest weight of the graph.</param>
    /// <returns></returns>
    private static float ManhattenDistance(Vertex v, Vertex u, object data = null)
    {
        return (Mathf.Abs(v.x - u.x) + Mathf.Abs(v.z - u.z)) * (float)data;
    }

    private static float DiagonalDistance(Vertex v, Vertex u, object data = null)
    {
        float dx = Mathf.Abs(v.x - u.x);
        float dy = Mathf.Abs(v.z - u.z);
        (float D, float D2) = ((float, float))data;
        return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);
    }

    private static float EuclideanDistance(Vertex v, Vertex u, object data = null)
    {
        float dx = Mathf.Abs(v.x - u.x);
        float dy = Mathf.Abs(v.z - u.z);
        return (float)data * Mathf.Sqrt(dx * dx + dy * dy);
    }
}