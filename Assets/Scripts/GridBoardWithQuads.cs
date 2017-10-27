using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoardWithQuads : MonoBehaviour
{

    public Transform gridPrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;

    [Range(0,1)]
    public float borderThickness;
    public int seed = 10;
    [Range(0,1)]
    public float obstacleRatio;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCords;

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
        shuffledTileCords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(),seed));

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
                newGrid.localScale = Vector3.one * (1 - borderThickness);
                newGrid.parent = mapHolder;
                Utility.RandomColor(ref newGrid);
            }


        int obstacleCount = (int)(mapSize.x * mapSize.y * obstacleRatio);
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            Vector3 obstaclePos = CoordToPosition(randomCoord.x, randomCoord.y);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePos + Vector3.back*.5f, Quaternion.identity) as Transform;
            Renderer _render = newObstacle.GetComponent<Renderer>();
            _render.material.color = Color.black;
        }

    }

    Vector3 CoordToPosition(int _x, int _y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + _x, -mapSize.y / 2 + 0.5f + _y, 0);
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