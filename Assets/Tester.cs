using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JStuff.Utilities;
using JStuff.Randomness;
using UnityEngine.UI;
using System.IO;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject textElement;
    public GameObject canvas;
    public GameObject HightMapGO;
    public Terrain Terrain;

    void Start()
    {
        MultiPopper<string, int> mp = new MultiPopper<string, int>((x, y) => x == y, (x, y) => x+y, "");

        mp.Add("", 3);
        mp.Add("", 3);

        Test(mp.MultiPop(3), 9);

        mp.Add("john", 3);
        mp.Add("john", 3);

        Test(mp.MultiPop(0), 3);

        mp.Add("john", 3);
        mp.Add("john", 3);
        mp.Add("maybe", 3);
        mp.Add("", 1);

        Test(mp.MultiPop(0), 7);

        mp.Add("", -5);
        mp.Add("", -5);
        mp.Add("john", 5);

        Test(mp.MultiPop(0), -5);

        MultiPopper<string, Vector2> mp2 = new MultiPopper<string, Vector2>((x, y) => x == y, (x, y) => x + y, "");

        mp2.Add("", new Vector2(1, 1));
        mp2.Add("", new Vector2(1, 1));

        Test(mp2.MultiPop(new Vector2(0,0)), new Vector2(2, 2));

        mp2.Add("john", new Vector2(1, 0));
        mp2.Add("john", new Vector2(1, 0));
        mp2.Add("daniel", new Vector2(0, 1));

        Test(mp2.MultiPop(new Vector2(0, 0)), new Vector2(1, 1));

        mp2.Add("john", new Vector2(1, 0));
        mp2.Add("john", new Vector2(1, 0));
        mp2.Add("daniel", new Vector2(0, 1));

        Test(mp2.MultiPop(new Vector2(1, 1)), new Vector2(2, 2));

        int[,] hills = Noise.ParticleDisposition(17, 1, 1, 2 * 17 * 17, 4, Noise.NormalStrategy);
        float[,] hillsInter = Noise.Interpolation(hills, 16, (float h) => 0.5f * (1 - Mathf.Cos(Mathf.PI * h)));

        File.WriteAllBytes(Application.persistentDataPath + "/" + "t1heightmapHills.png",
            (byte[])Noise.GenerateHightMap(hillsInter.ToInt()).EncodeToPNG());

        int[,] moreNoise = Noise.ParticleDisposition(17, 1, 1, 2 * 17 * 17, 4, Noise.RandomStrategy);
        float[,] noiseInter = Noise.Interpolation(moreNoise, 16, (float h) => 0.5f * (1 - Mathf.Cos(Mathf.PI * h)));
        File.WriteAllBytes(Application.persistentDataPath + "/" + "t2heightmapNoise.png",
            (byte[])Noise.GenerateHightMap(noiseInter.ToInt()).EncodeToPNG());

        int[,] folded = Utilities.Fold(noiseInter.ToInt(), hillsInter.ToInt());
        File.WriteAllBytes(Application.persistentDataPath + "/" + "t3heightmapFinished.png",
            (byte[])Noise.GenerateHightMap(folded).EncodeToPNG());

        int[,] noised = Noise.ParticleDisposition(folded, 10, 10, 1 * 256 * 256, 1000, Noise.RandomStrategy);
        File.WriteAllBytes(Application.persistentDataPath + "/" + "t4heightmapFinishedNoised.png",
            (byte[])Noise.GenerateHightMap(noised).EncodeToPNG());

        int[,] finished = Noise.ParticleDisposition(noised, 225, 225, 225, 2000, MountainStrategy);
        finished = Noise.ParticleDisposition(finished, 300, 300, 10, 2000, MountainStrategy);
        finished = Noise.ParticleDisposition(finished, 150, 150, 1000, 2000, MountainStrategy);
        finished = Noise.ParticleDisposition(finished, 150, 150, 1000, 2000, MountainStrategy);
        finished = Noise.ParticleDisposition(finished, 150, 150, 1000, 2000, MountainStrategy);

        File.WriteAllBytes(Application.persistentDataPath + "/" + "t5heightmapFinishedMountains.png",
            (byte[])Noise.GenerateHightMap(finished, 200, 2000).EncodeToPNG());

        int[,] trees = Noise.ParticleDisposition(128, 1, 1, 500, 1, Noise.RandomStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 500, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 500, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 500, 1, Noise.NormalStrategy);

        trees = Noise.ParticleDisposition(trees, 1, 1, 50, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 50, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 50, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 50, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 50, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 50, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 50, 1, Noise.NormalStrategy);

        trees = Noise.ParticleDisposition(trees, 1, 1, 1000, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 1000, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 1000, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 1000, 1, Noise.NormalStrategy);
        trees = Noise.ParticleDisposition(trees, 1, 1, 1000, 1, Noise.NormalStrategy);
        //trees = Noise.ParticleDisposition(trees, 1, 1, 1000, 1, Noise.NormalStrategy);
        //trees = Noise.ParticleDisposition(trees, 1, 1, 1000, 1, Noise.NormalStrategy);
        //trees = Noise.ParticleDisposition(trees, 1, 1, 1000, 1, Noise.NormalStrategy);

        File.WriteAllBytes(Application.persistentDataPath + "/" + "t6trees.png",
            (byte[])Noise.GenerateHightMap(trees, 0, 1).EncodeToPNG());

        PlantTrees(trees);

        //ParticleDispositionImage(160, 1, 1, 160 * 160, 150, MountainStrategy, "mountainA");
        //ParticleDispositionImage(160, 1, 5, 160 * 160, 150, MountainStrategy, "mountainB");
        //ParticleDispositionImage(160, 1, 20, 160 * 160, 150, MountainStrategy, "mountainC");
        //ParticleDispositionImage(160, 1, 40, 500, 150, MountainStrategy, "mountainD");
        //ParticleDispositionImage(160, 1, 1, 160 * 160, 150, NormalStrategy, "normalA");
        //ParticleDispositionImage(160, 1, 5, 160 * 160, 150, NormalStrategy, "normalB");
        //ParticleDispositionImage(160, 1, 20, 160 * 160, 150, NormalStrategy, "normalC");
        //
        //ParticleDispositionImage(128, 1, 20, 128 * 128, 150, NormalStrategy, "forprint");
        //
        //int[,] intmap = Noise.ToInt(Noise.Interpolation(map, 16, (float h) => h));
        ////    (float h) => 6 * Mathf.Pow(h, 5) - 15 * Mathf.Pow(h, 4) + 10 * Mathf.Pow(h, 3)));
        //File.WriteAllBytes(Application.persistentDataPath + "/" + "heightmaptestinterpolated0.png",
        //    (byte[])Noise.GenerateHightMap(intmap).EncodeToPNG());
        //intmap = Noise.ParticleDisposition(intmap, 100, 100, 500, 1000, MountainStrategy);
        //intmap = Noise.ParticleDisposition(intmap, 100, 100, 500, 1000, MountainStrategy);
        //intmap = Noise.ParticleDisposition(intmap, 100, 100, 500, 1000, MountainStrategy);
        //intmap = Noise.ParticleDisposition(intmap, 100, 100, 500, 1000, MountainStrategy);
        //
        //intmap = Noise.ParticleDisposition(intmap, 100, 50, intmap.GetLength(0) * intmap.GetLength(0) * 2, 1000, NormalStrategy);
        //File.WriteAllBytes(Application.persistentDataPath + "/" + "heightmaptestinterpolated1.png",
        //    (byte[])Noise.GenerateHightMap(intmap).EncodeToPNG());
        //
        //Debug.Log(intmap.GetLength(0));
        //
        //Debug.Log(Application.persistentDataPath);





        //map = Noise.ParticleDisposition(16, 1, 1, 10, 2);
        //
        //Debug.Log(Application.persistentDataPath);
        //
        //File.WriteAllBytes(Application.persistentDataPath + "/" + "heightmaptest2.png", (byte[])Noise.GenerateHightMap(map).EncodeToPNG());
        //
        //map = Noise.ParticleDisposition(16, 1, 1, 1, 100);
        //
        //Debug.Log(Application.persistentDataPath);
        //
        //File.WriteAllBytes(Application.persistentDataPath + "/" + "heightmaptest3.png", (byte[])Noise.GenerateHightMap(map).EncodeToPNG());


        //Debug.Log(map.ToString());
        Debug.Log("Finished generating map.");
        Debug.Log(Application.persistentDataPath);

        //Noise.LoadTerrain(Noise.GenerateHightMap(map), Terrain.terrainData);

        //HightMapGO.GetComponent<SpriteRenderer>().sprite =
        //    Sprite.Create(Noise.GenerateHightMap(map), new Rect(0, 0, map.GetLength(0), map.GetLength(1)), Vector2.zero);
    }

    void PlantTrees(int[,] trees)
    {
        for (int i = 0; i < trees.GetLength(0); i++)
        {
            for (int j = 0; j < trees.GetLength(1); j++)
            {
                if (trees[i, j] == 1)
                {
                    GameObject tree = Instantiate(Resources.Load("Prefabs/tree")) as GameObject;
                    tree.transform.position = new Vector3(i, tree.transform.position.y, j);
                }
            }
        }
    }

    void ParticleDispositionImage(int size, int dispHight, int particleHight, int amount, int maxHeight,
        Noise.DropStrategy strategy,
        string name)
    {
        int[,] mapMountain = Noise.ParticleDisposition(size, dispHight, particleHight, amount, maxHeight, strategy);
        File.WriteAllBytes(Application.persistentDataPath + "/" + name + "1.png",
            (byte[])Noise.GenerateHightMap(mapMountain).EncodeToPNG());
        mapMountain = Noise.ParticleDisposition(size, dispHight, particleHight, amount, maxHeight, strategy);
        File.WriteAllBytes(Application.persistentDataPath + "/" + name + "2.png",
            (byte[])Noise.GenerateHightMap(mapMountain).EncodeToPNG());
        mapMountain = Noise.ParticleDisposition(size, dispHight, particleHight, amount, maxHeight, strategy);
        File.WriteAllBytes(Application.persistentDataPath + "/" + name + "3.png",
            (byte[])Noise.GenerateHightMap(mapMountain).EncodeToPNG());
    }

    void Test(object actual, object expected)
    {
        Debug.Log("Test: " + (actual.ToString() == expected.ToString()) + ". Actual value: " + actual + ". Expected value: " + expected);
    }

    private int[,] NormalStrategy(int[,] map, int dispHight, int particleHight, int amount, int maxHeight,
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

    private int[,] MountainStrategy(int[,] map, int dispHight, int particleHight, int amount, int maxHeight,
        System.Func<int[,], int, int, (int, int, int)> increase,
        System.Func<int[,], int, int, (int, int)[]> unbalancedNeightbors,
        System.Func<int[,], int, int, (int, int)> brownianMovement)
    {
        int size = map.GetLength(0);

        (int x, int y) point = (Random.Range(0, size), Random.Range(0, size));
        (int x, int y) point2 = point;
        for (int i = 0; i < amount / 3; i++)
        {
            (int incx, int incy, int inc) = increase(map, point.x, point.y);
            map[incx, incy] += inc;
            point = brownianMovement(map, point.x, point.y);
        }
        point = point2;
        for (int i = 0; i < amount / 3; i++)
        {
            (int incx, int incy, int inc) = increase(map, point.x, point.y);
            map[incx, incy] += inc;
            point = brownianMovement(map, point.x, point.y);
        }
        point = point2;
        for (int i = 0; i < amount / 3; i++)
        {
            (int incx, int incy, int inc) = increase(map, point.x, point.y);
            map[incx, incy] += inc;
            point = brownianMovement(map, point.x, point.y);
        }

        return map;
    }
}