using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static int[,] Fold(int[,] a1, int[,] a2)
    {
        if (a1.GetLength(0) != a2.GetLength(0) || a1.GetLength(1) != a2.GetLength(1))
            throw new System.Exception("Arrays are not same height or width.");

        int[,] retval = new int[a1.GetLength(0), a1.GetLength(1)];
        for (int i = 0; i < a1.GetLength(0); i++)
        {
            for (int j = 0; j < a1.GetLength(1); j++)
            {
                retval[i,j] = a1[i, j] + a2[i, j];
            }
        }
        return retval;
    } 

    public static int[,] Add(int[,] a, int number)
    {
        int[,] retval = new int[a.GetLength(0), a.GetLength(1)];
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                retval[i, j] = a[i, j] + number;
            }
        }
        return retval;
    }

    public static int[,] Copy(this int[,] a)
    {
        int[,] retval = new int[a.GetLength(0), a.GetLength(1)];
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                retval[i, j] = a[i, j];
            }
        }
        return retval;
    }

    public static int[,] ToInt(this float[,] map)
    {
        int[,] retval = new int[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < retval.GetLength(0); x++)
        {
            for (int y = 0; y < retval.GetLength(1); y++)
            {
                retval[x, y] = Mathf.FloorToInt((map[x, y] + 1) * 100);
            }
        }
        return retval;
    }
}