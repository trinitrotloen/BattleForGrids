using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoardWithQuads : MonoBehaviour
{

    public Transform gridPrefab , obstaclePrefab;
    public Vector2 mapSize;
    public float tileSize;

    [Range(0, 1)]
    public float borderThickness, obstacleRatio;
    public int seed = 10;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCords;
    Coord mapCentre;

    private void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        allTileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        shuffledTileCords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));
        mapCentre = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);

        string holderName = "GeneratedMap";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }


        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;
        for (int x = 0; x < mapSize.x; x++)
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 pos = CoordToPosition(x, y);
                Transform newGrid = Instantiate(gridPrefab, pos, Quaternion.identity) as Transform;
                newGrid.localScale = Vector3.one * (1 - borderThickness)*tileSize;
                newGrid.parent = mapHolder;
                Utility.RandomColor(ref newGrid);
            }


        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        int obstacleCount = (int)(mapSize.x * mapSize.y * obstacleRatio);
        int currentObstacleCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;
            if (!randomCoord.Equals(mapCentre) && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
                {
                Vector3 obstaclePos = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePos + Vector3.back * .5f, Quaternion.identity) as Transform;
                newObstacle.localScale = Vector3.one * (1 - borderThickness) * tileSize;
                newObstacle.parent = mapHolder;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

    }

    bool MapIsFullyAccessible(bool[,] _obstacleMap, int _obstacleCount)
    {
        bool[,] mapFlags = new bool[_obstacleMap.GetLength(0), _obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCentre);
        mapFlags[mapCentre.x, mapCentre.y] = true;

        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < _obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < _obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !_obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - _obstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }


    Vector3 CoordToPosition(int _x, int _y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + _x, -mapSize.y / 2 + 0.5f + _y, 0)*tileSize;
    }

    public Coord GetRandomCoord()
    {
        Coord rndCoord = shuffledTileCords.Dequeue();  //remove first member from queue
        shuffledTileCords.Enqueue(rndCoord); //insert to the queue as a last member
        return rndCoord;
    }

    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

}