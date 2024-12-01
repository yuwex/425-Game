using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameBoard : MonoBehaviour
{

    public int BoardLength, TileSize;
    private GameObject[,] Tiles;

    [Header("Wall Stuff xD")]

    public Mesh NoConnections;
    public Mesh TwoConnections;
    public Mesh Corner;
    public Mesh ThreeConnections;
    public Mesh FourConnections;

    public GameObject ConnectWallPrefab;

    [Header("Terrain Objects")]

    public List<GameObject> TerrainObjects;
    public int numObjects;
    public GameObject terrainParent;

    void Awake()
    {
        Tiles = new GameObject[BoardLength, BoardLength];
    }

    public GameObject GetBoard(int x, int y)
    {
        if (x >= BoardLength || y >= BoardLength || x < 0 || y < 0) return null;
        return Tiles[x, y];
    }

    public void SetBoard(int x, int y, GameObject g)
    {
        if (!(x >= BoardLength || y >= BoardLength || x < 0 || y < 0))
        {
            Tiles[x, y] = g;
        }
    }

    private void Start()
    {
        TerrainSpawner();
    }
    void Update()
    {
        // if (Input.GetMouseButton(0)) {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit)) {
        //         Debug.Log(NormalizePos(hit.point) + " vs " + hit.point);

        //         // Debug.Log("(" + x + ", " + z + ") vs " + hit.point);

        //     }
        // }
    }

    public (int, int) GetCoordinateFromPos(Vector3 pos)
    {
        var offset = BoardLength / 2 * TileSize;

        return (
            (int)Math.Floor((pos.x + offset) / 5),
            (int)Math.Floor((pos.z + offset) / 5)
        );
    }

    public Vector3 NormalizePos(Vector3 pos)
    {
        var (x, z) = GetCoordinateFromPos(pos);
        float offset = BoardLength / 2f * TileSize - (TileSize / 2f);

        return new Vector3(x * 5 - offset, 0, z * 5 - offset);
    }

    public GameObject GetObjectFromPos(Vector3 pos)
    {
        var (x, y) = GetCoordinateFromPos(pos);
        return GetBoard(x, y);
    }

    public void SetObjectAtPos(Vector3 pos, GameObject g)
    {
        var (x, y) = GetCoordinateFromPos(pos);
        SetBoard(x, y, g);
    }

    public void UpdateWalls(Vector3 pos)
    {
        (int x, int y) mid = GetCoordinateFromPos(pos);
        (int x, int y)[] neighbors = { (0, 1), (1, 0), (-1, 0), (0, -1) };
        foreach ((int x, int y) diff in neighbors)
        {
            (int x, int y) coord = (mid.x + diff.x, mid.y + diff.y);
            var obj = GetBoard(coord.x, coord.y);
            if (obj != null && !obj.CompareTag("Terrain"))
            {
                UpdateWall(coord);
            }
        }
        if (GetBoard(mid.x, mid.y) != null)
        {
            UpdateWall(mid);
        }
    }

    void UpdateWall((int x, int y) mid)
    {
        GameObject midWall = GetBoard(mid.x, mid.y);
        Transform[] Children = midWall.GetComponentsInChildren<Transform>();

        foreach (Transform Child in Children)
        {
            if (Child.CompareTag("ConnectorWall"))
            {
                Destroy(Child.gameObject);
            }
        }

        (int x, int y)[] diffs = { (0, 1), (1, 0), (0, -1), (-1, 0) };
        List<(int x, int y)> neighbors = new List<(int, int)>();
        foreach ((int x, int y) diff in diffs)
        {
            (int x, int y) coord = (mid.x + diff.x, mid.y + diff.y);
            var obj = GetBoard(coord.x, coord.y);
            if (obj != null && !obj.CompareTag("Terrain"))
            {
                neighbors.Add(diff);
            }
        }

        if (midWall.CompareTag("Wall"))
        {

            if (neighbors.Count == 0)
            {
                UpdateWallTexture(midWall, NoConnections, (0, 1));
            }
            else if (neighbors.Count == 1)
            {
                UpdateWallTexture(midWall, NoConnections, (0, 1));
            }
            else if (neighbors.Count == 2)
            {
                if (neighbors[0].x == -neighbors[1].x && neighbors[0].y == -neighbors[1].y)
                {
                    UpdateWallTexture(midWall, TwoConnections, neighbors[0]);
                }
                else
                {
                    if (neighbors[0] == (0, 1) && neighbors[1] == (-1, 0))
                    {
                        UpdateWallTexture(midWall, Corner, neighbors[1]);
                    }
                    else
                    {
                        UpdateWallTexture(midWall, Corner, neighbors[0]);
                    }
                }
            }
            else if (neighbors.Count == 3)
            {
                foreach (var diff in diffs)
                {
                    if (!neighbors.Contains(diff))
                    {
                        UpdateWallTexture(midWall, ThreeConnections, diff);
                        midWall.transform.Rotate(0f, -90f, 0f);
                    }
                }
            }
            else
            {
                UpdateWallTexture(midWall, FourConnections, (0, 1));
            }
        }

        foreach ((int x, int y) neighbor in neighbors)
        {
            var connectPos = new Vector3(midWall.transform.position.x + neighbor.x * 2.5f, midWall.transform.position.y, midWall.transform.position.z + neighbor.y * 2.5f);
            Quaternion direction = new Quaternion();
            switch (neighbor)
            {
                case (0, 1):
                    direction = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case (1, 0):
                    direction = Quaternion.Euler(0f, 90f, 0f);
                    break;
                case (0, -1):
                    direction = Quaternion.Euler(0f, 180f, 0f);
                    break;
                case (-1, 0):
                    direction = Quaternion.Euler(0f, 270f, 0f);
                    break;
            }
            GameObject Connector = Instantiate(ConnectWallPrefab, connectPos, direction);
            Connector.transform.parent = midWall.transform;
        }
    }

    void UpdateWallTexture(GameObject wall, Mesh mesh, (int x, int y) direction)
    {
        wall.GetComponent<MeshFilter>().sharedMesh = mesh;
        wall.GetComponent<MeshCollider>().sharedMesh = mesh;
        switch (direction)
        {
            case (0, 1):
                wall.transform.Rotate(0f, 0f - wall.transform.eulerAngles.y, 0f);
                break;
            case (1, 0):
                wall.transform.Rotate(0f, 90f - wall.transform.eulerAngles.y, 0f);
                break;
            case (0, -1):
                wall.transform.Rotate(0f, 180f - wall.transform.eulerAngles.y, 0f);
                break;
            case (-1, 0):
                wall.transform.Rotate(0f, 270f - wall.transform.eulerAngles.y, 0f);
                break;
        }
    }

    public void TerrainSpawner()
    {
        int objectsSpawned = 0;

        while (objectsSpawned < numObjects)
        {
            int x = UnityEngine.Random.Range(0, BoardLength);
            int y = UnityEngine.Random.Range(0, BoardLength);
            int boardLength = BoardLength / 2;

            bool inTowerArea = x >= (boardLength - 2) && x <= (boardLength + 2) && y >= (boardLength - 2) && y <= (boardLength + 2);

            if (Tiles[x, y] == null && !inTowerArea)
            {
                GameObject randomObject = TerrainObjects[UnityEngine.Random.Range(0, TerrainObjects.Count)];

                float offset = (BoardLength * TileSize) / 2.0f - (TileSize / 2.0f);
                Vector3 spawnPos = new Vector3(x * TileSize - offset, 0, y * TileSize - offset);

                GameObject spawnedObject = Instantiate(randomObject, spawnPos, Quaternion.identity);
                spawnedObject.transform.parent = terrainParent.transform;
                SetBoard(x, y, spawnedObject);

                objectsSpawned++;
            }
        }
    }



    public bool PathExists(int startX, int startY, int targetX, int targetY)
    {
        // Bounds check
        if (startX < 0 || startY < 0 || targetX < 0 || targetY < 0) return false;
        if (startX >= BoardLength || startY >= BoardLength || targetX >= BoardLength || targetY >= BoardLength) return false;

        // BFS queue
        Queue<(int, int)> queue = new Queue<(int, int)>();
        HashSet<(int, int)> visited = new HashSet<(int, int)>();
        queue.Enqueue((startX, startY));

        // Define movement directions (up, right, down, left)
        (int, int)[] directions = { (0, 1), (1, 0), (0, -1), (-1, 0) };

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            // If we reached the target, return true
            if (x == targetX && y == targetY) return true;

            // Skip if visited or blocked
            if (visited.Contains((x, y))) continue;
            visited.Add((x, y));

            // Check all neighbors
            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx, ny = y + dy;

                // Bounds check and walkable condition
                if (nx >= 0 && ny >= 0 && nx < BoardLength && ny < BoardLength)
                {
                    var neighborTile = GetBoard(nx, ny);
                    if (neighborTile == null)
                    {
                        queue.Enqueue((nx, ny));
                    }
                }
            }
        }

        // No path found
        return false;
    }

    public List<(int, int)> GetBaseCoords()
    {
        int middle = BoardLength / 2;
        return new List<(int, int)> { (middle, middle), (middle - 1, middle), (middle, middle - 1), (middle - 1, middle - 1) };
    }
}
