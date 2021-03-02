using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Vertex
{
    public float x;
    public float z;
}

/// <summary>
/// A directed graph data-structure.
/// </summary>
[System.Serializable]
public class Graph
{
    [SerializeField]
    private List<Vertex> vertices;
    [SerializeField]
    private Dictionary<Vertex, List<Vertex>> edges;
    [SerializeField]
    private Dictionary<(Vertex,Vertex), float> weights;

    //private List<Vertex> disabledVertices;

    public Graph()
    {
        if (vertices == null || edges == null || weights == null)
        {
            vertices = new List<Vertex>();
            edges = new Dictionary<Vertex, List<Vertex>>();
            weights = new Dictionary<(Vertex, Vertex), float>();
        }
        //disabledVertices = new List<Vertex>();
    }

    //public void DisableVertex(float nx, float nz)
    //{
    //    disabledVertices.Add(new Vertex { x = nx, z = nz });
    //}
    //
    //public void EnableVertex(float nx, float nz)
    //{
    //    if (disabledVertices.Contains(new Vertex { x = nx, z = nz }))
    //        disabledVertices.Remove(new Vertex { x = nx, z = nz });
    //}

    public Vertex[] GetNeighbors(Vertex v)
    {
        if (!edges.ContainsKey(v))
        {
            Debug.LogError("Vertex v not in edges, v = " + v.x + ";" + v.z);
            foreach (Vertex u in vertices)
            {
                Debug.Log(u.x + ";" + u.z);
            }
        }

        //return edges[v].Except(disabledVertices).ToArray();
        return edges[v].ToArray();
    }

    public Vertex[] GetNeighbors(Vertex v, Vertex u)
    {
        bool contains = false;
        if (edges[u].Contains(v))
        {
            contains = true;
            edges[u].Remove(v);
        }

        Vertex[] retval = edges[u].ToArray();

        if (contains)
            edges[u].Add(v);

        //return retval.Except(disabledVertices).ToArray();
        return retval.ToArray();
    }

    public Vertex[] GetVertices()
    {
        return vertices.ToArray();
    }

    public void AddVertex(Vertex v)
    {
        if (vertices.Contains(v))
            return;
        vertices.Add(v);
        edges.Add(v, new List<Vertex>());
    }

    public void AddVertex(float nx, float nz)
    {
        Vertex v = new Vertex() { x = nx, z = nz };
        if (vertices.Contains(v))
            return;
        vertices.Add(v);
        edges.Add(v, new List<Vertex>());
    }

    public void AddEdge(Vertex v, Vertex u, float w)
    {
        if (edges[v].Contains(u))
            return;

        if (!vertices.Contains(v))
        {
            Debug.LogError("Vertex v cannot have a neighbour, since v is not contained in the graph. v = " + v.x + ";" + v.z);
            return;
        }
        if (!vertices.Contains(u))
        {
            Debug.LogError("Vertex u cannot be a neighbour, since u is not contained in the graph. u = " + u.x + ";" + u.z);
            return;
        }
            

        if (w <= 0)
            Debug.Log("WARNING: Edge added with non-positive wieght.");

        edges[v].Add(u);
        weights.Add((v, u), w);
    }

    public float GetWeight(Vertex v, Vertex u)
    {
        return weights[(v, u)];
    }
}