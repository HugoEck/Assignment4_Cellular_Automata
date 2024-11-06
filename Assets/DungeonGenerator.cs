using System.Collections;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Parameters")]
    public int width = 50;
    public int height = 50;
    public float fillProbability = 0.4f; // Chance to initialize a cell as a wall
    public int iterations = 4; // Number of CA iterations

    [Header("Tiles")]
    public GameObject wallTile;
    public GameObject floorTile;

    private int[,] map; // 2D grid for the dungeon

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

        // Adjust camera after generating the dungeon
        Camera.main.GetComponent<CameraController>().AdjustCamera();
    }

    void InitializeMap()
    {
        // Randomly assign wall or floor based on fill probability
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

                // Rules for wall/floor transition based on neighbor count
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
                if (i == 0 && j == 0) continue; // Ignore the cell itself
                int nx = x + i, ny = y + j;

                // Count as wall if out of bounds or it's a wall cell
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
