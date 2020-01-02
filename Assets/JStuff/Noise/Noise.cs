//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    public int NoiseRandom(int value)
    {
        return Random.Range(-1, 1) + value;
    }

    //public enum ParticleDopStrategy
    //{
    //    Normal,
    //    Random
    //}

    private static int[,] array1 = { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };

    public delegate int[,] DropStrategy(int[,] map, int dispHight, int particleHight, int amount, int maxHeight,
        System.Func<int[,], int, int, (int, int, int)> inc,
        System.Func<int[,], int, int, (int, int)[]> unba,
        System.Func<int[,], int, int, (int, int)> brownmove);

    public delegate int[,] DropStrategyAlternative<T>(int[,] map, int dispHight, int particleHight, int maxHeight,
        System.Func<int[,], int, int, (int, int, int)> inc,
        System.Func<int[,], int, int, (int, int)[]> unba,
        System.Func<int[,], int, int, (int, int)> brownmove,
        T context);

    public static int[,] ParticleDisposition(int size, int dispHight, int particleHight, int amount, int maxHeight,
        DropStrategy strategy)
    {
        int[,] retval = new int[size, size];

        retval = ParticleDispositionF(retval, dispHight, particleHight, amount, maxHeight, strategy);

        return retval;
    }

    public static int[,] ParticleDisposition(int[,] map, int dispHight, int particleHight, int amount, int maxHeight,
        DropStrategy strategy)
    {
        int[,] retval = map.Copy();

        retval = ParticleDispositionF(retval, dispHight, particleHight, amount, maxHeight, strategy);

        return retval;
    }

    //public static int[,] ParticleDisposition(int size, int dispHight, int particleHight, int amount, int maxHeight,
    //    ParticleDopStrategy strategy)
    //{
    //    int[,] retval = new int[size, size];
    //
    //    switch (strategy)
    //    {
    //        case ParticleDopStrategy.Normal:
    //            return ParticleDispositionF(retval, dispHight, particleHight, amount, maxHeight, NormalStrategy);
    //            break;
    //        case ParticleDopStrategy.Random:
    //            return ParticleDispositionF(retval, dispHight, particleHight, amount, maxHeight, RandomStrategy);
    //            break;
    //        default:
    //            throw new System.Exception("Oof");
    //    }
    //}

    private static int[,] ParticleDispositionF(int[,] map, int dispHight, int particleHight, int amount, int maxHeight,
        DropStrategy strategy)
    {
        int giveUpMax = 10;
        int giveUp = giveUpMax;

        (int, int, int) Increase(int[,] nmap, int x, int y)
        {
            (int, int)[] nightbors = UnbalancedNeightbors(nmap, x, y);
            int i = Mathf.Min(maxHeight - nmap[x, y], particleHight);
            if (nightbors == null)
            {
                if (i == 0)
                {
                    giveUp--;
                    if (giveUp <= 0)
                    {
                        Debug.Log("Particle Disposition Increase failed to find a suitible spot for drop.");
                        return (x, y, i);
                    }
                    (int nx, int ny) = BrownianMovement(nmap, x, y);
                    return Increase(nmap, nx, ny);
                }
                return (x, y, i);
            }
            int index = Random.Range(0, nightbors.Length - 1);
            giveUp = giveUpMax;
            return Increase(nmap, nightbors[index].Item1, nightbors[index].Item2);
        }

        (int, int)[] UnbalancedNeightbors(int[,] nmap, int x, int y)
        {
            List<(int, int)> retval = new List<(int, int)>();
            if (x > 0 && nmap[x, y] >= nmap[x - 1, y] + dispHight)
                retval.Add((x - 1, y));
            if (y > 0 && nmap[x, y] >= nmap[x, y - 1] + dispHight)
                retval.Add((x, y - 1));
            if (x < nmap.GetLength(0) - 1 && nmap[x, y] >= nmap[x + 1, y] + dispHight)
                retval.Add((x + 1, y));
            if (y < nmap.GetLength(1) - 1 && nmap[x, y] >= nmap[x, y + 1] + dispHight)
                retval.Add((x, y + 1));
            return (retval.Count > 0) ? retval.ToArray() : null;
        }

        (int, int) BrownianMovement(int[,] nmap, int x, int y)
        {
            bool found = false;
            while (!found)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        if (x < nmap.GetLength(0) - 1)
                            return (x + 1, y);
                        break;
                    case 1:
                        if (y < nmap.GetLength(1) - 1)
                            return (x, y + 1);
                        break;
                    case 2:
                        if (x > 0)
                            return (x - 1, y);
                        break;
                    case 3:
                        if (y > 0)
                            return (x, y - 1);
                        break;
                    default:
                        throw new System.Exception("Outside range");
                }
            }
            throw new System.Exception("Oof");
        }

        map = strategy(map, dispHight, particleHight, amount, maxHeight,
            Increase, UnbalancedNeightbors, BrownianMovement);

        return map;
    }

    public static int[,] NormalStrategy(int[,] map, int dispHight, int particleHight, int amount, int maxHeight,
    System.Func<int[,], int, int, (int, int, int)> increase,
    System.Func<int[,], int, int, (int, int)[]> unbalancedNeightbors,
    System.Func<int[,], int, int, (int, int)> brownianMovement)
    {
        int size = map.GetLength(0);

        (int x, int y) point = (Random.Range(0, size), Random.Range(0, size));
        for (int i = 0; i < amount; i++)
        {
            (int incx, int incy, int inc) = increase(map, point.x, point.y);
            map[incx, incy] += inc;
            point = brownianMovement(map, point.x, point.y);
        }

        return map;
    }

    public static int[,] RandomStrategy(int[,] map, int dispHight, int particleHight, int amount, int maxHeight,
    System.Func<int[,], int, int, (int, int, int)> increase,
    System.Func<int[,], int, int, (int, int)[]> unbalancedNeightbors,
    System.Func<int[,], int, int, (int, int)> brownianMovement)
    {
        int size = map.GetLength(0);

        (int x, int y) point = (Random.Range(0, size), Random.Range(0, size));
        for (int i = 0; i < amount; i++)
        {
            (int incx, int incy, int inc) = increase(map, point.x, point.y);
            map[incx, incy] += inc;
            point = (Random.Range(0, size), Random.Range(0, size));
        }

        return map;
    }

    public static float[,] Interpolation(int[,] map, int detail, System.Func<float,float> function)
    {
        float[,] retval = new float[map.GetLength(0) * detail - detail, map.GetLength(1) * detail - detail];

        //6t^5 - 15t^4 + 10t^3
        //t

        for (int x = 0; x < retval.GetLength(0); x++)
        {
            for (int y = 0; y < retval.GetLength(1); y++)
            {
                float x_ = (float)x / detail;
                float y_ = (float)y / detail;

                float a00 = map[Mathf.FloorToInt(x_), Mathf.FloorToInt(y_)];
                float a10 = map[Mathf.CeilToInt(x_), Mathf.FloorToInt(y_)];
                float a01 = map[Mathf.FloorToInt(x_), Mathf.CeilToInt(y_)];
                float a11 = map[Mathf.CeilToInt(x_), Mathf.CeilToInt(y_)];

                //System.Func<float, float> f = (float h) => 6 * Mathf.Pow(h, 5) - 15 * Mathf.Pow(h, 4) + 10 * Mathf.Pow(h, 3);
                //System.Func<float, float> f = (float h) => h;
                //System.Func<float, float> f = (float h) => 0.5f * (1 - Mathf.Cos(Mathf.PI * h));

                retval[x, y] = ValueInterpolation(
                    ValueInterpolation(a00, a10, (float)(x % detail) / detail, function),
                    ValueInterpolation(a01, a11, (float)(x % detail) / detail, function),
                    (float)(y % detail) / detail, function);
                //Debug.Log(retval[x, y]);
                //if (a00 != a10)
                //    Debug.Log((float)ValueInterpolation(a00, a10, (float)(x % (float)detail) / (float)detail) + 
                //    ". a0;a1: " + a00 + ";" + a10 + ". frac: " + (float)(x % detail) / detail);
            }
        }

        return retval;
    }

    public static float ValueInterpolation(float a0, float a1, float t, System.Func<float, float> f)
    {
        return UnitLerp(a0, a1, f(t));
    }
    public static float ValueInterpolation(float a0, float a1, float t)
    {
        return UnitLerp(a0, a1, t);
    }

    //public static float Rescale(x) { }

    public static float UnitClamp(float x)
    {
        return (x < 0) ? ((x > 1) ? 1 : x) : 0;
    }

    public static float UnitWrap(float x)
    {
        return x - Mathf.Floor(x);
    }

    public static float UnitStep(float x)
    {
        return (x > 0) ? 1 : 0;
    }

    public static float UnitRescale(float min, float max, float x)
    {
        if (max < min || (x < min || x > max))
            throw new System.Exception("x must be between min and max and max must be larger than min.");

        return (x - min) / (max - min);
    }

    public static float UnitLerp(float a0, float a1, float t)
    {
        return a0 * (1 - t) + a1 * t;
    }



    public static Texture2D GenerateHightMap(int[,] map)
    {
        Texture2D retval = new Texture2D(map.GetLength(0), map.GetLength(1));

        int maxVal = 0;
        int minVal = int.MaxValue;

        foreach (int i in map)
        {
            if (i > maxVal)
                maxVal = i;
            if (i < minVal)
                minVal = i;
        }

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                float value = ((float)map[i, j] - (float)minVal)/((float)maxVal - (float)minVal);
                retval.SetPixel(i, j, new Color(value, value, value));
            }
        }
        retval.Apply();
        return retval;
    }

    public static Texture2D GenerateHightMap(int[,] map, int floor, int ceil)
    {
        Texture2D retval = new Texture2D(map.GetLength(0), map.GetLength(1));

        int maxVal = ceil;
        int minVal = floor;

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                float value = ((float)map[i, j] - (float)minVal) / ((float)maxVal - (float)minVal);
                retval.SetPixel(i, j, new Color(value, value, value));
            }
        }
        retval.Apply();
        return retval;
    }

    public static void LoadTerrain(Texture2D texture2D, TerrainData aTerrain)
    {
        int h = aTerrain.heightmapResolution;
        int w = aTerrain.heightmapResolution;
        float[,] data = new float[h, w];

        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                float v = texture2D.GetPixel(w, h).r;
            }
        }

        //using (var file = System.IO.File.OpenRead(aFileName))
        //using (var reader = new System.IO.BinaryReader(file))
        //{
        //    for (int y = 0; y < h; y++)
        //    {
        //        for (int x = 0; x < w; x++)
        //        {
        //            float v = (float)reader.ReadUInt16() / 0xFFFF;
        //            data[y, x] = v;
        //        }
        //    }
        //}
        aTerrain.SetHeights(0, 0, data);
    }
}