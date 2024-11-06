using System.Collections;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Parameters")]
    public int width = 50;
    public int height = 50;
    public float fillProbability = 0.4f;
    public int iterations = 4;

    [Header("Tiles")]
    public GameObject wallTile;
    public GameObject floorTile;

    private int[,] map;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        map = new int[width, height];
        InitializeMap();

        for (int i = 0; i < iterations; i++)
        {
            map = RunCellularAutomataStep(map);
        }

        DrawMap();

        Camera.main.GetComponent<CameraController>().AdjustCamera();
    }

    void InitializeMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (Random.value < fillProbability) ? 1 : 0; // 1 for wall, 0 for floor
            }
        }
    }

    int[,] RunCellularAutomataStep(int[,] oldMap)
    {
        int[,] newMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWalls = CountWallNeighbors(oldMap, x, y);

                // Rules
                if (oldMap[x, y] == 1)
                {
                    newMap[x, y] = (neighborWalls >= 4) ? 1 : 0;
                }
                else
                {
                    newMap[x, y] = (neighborWalls >= 5) ? 1 : 0;
                }
            }
        }
        return newMap;
    }

    int CountWallNeighbors(int[,] map, int x, int y)
    {
        int count = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                int nx = x + i, ny = y + j;

                if (nx < 0 || ny < 0 || nx >= width || ny >= height)
                {
                    count++;
                }
                else
                {
                    count += map[nx, ny];
                }
            }
        }
        return count;
    }

    void DrawMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                if (map[x, y] == 1)
                {
                    Instantiate(wallTile, pos, Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(floorTile, pos, Quaternion.identity, transform);
                }
            }
        }
    }
}
