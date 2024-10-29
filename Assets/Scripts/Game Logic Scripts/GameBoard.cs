using System;
using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        Tiles = new GameObject[BoardLength, BoardLength];
    }

    public GameObject GetBoard(int x, int y)
    {
        return Tiles[x, y];
    }

    public void SetBoard(int x, int y, GameObject g)
    {
        Tiles[x, y] = g;
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
        var mid = GetCoordinateFromPos(pos);
        (int, int)[] neighbors = { (0, 1), (1, 0), (-1, 0), (0, -1) };
        foreach ((int, int) diff in neighbors)
        {
            (int, int) coord = (mid.Item1 + diff.Item1, mid.Item2 + diff.Item2);
            var obj = GetBoard(coord.Item1, coord.Item2);
            if (obj != null && obj.tag == "Wall")
            {
                UpdateWall(coord);
            }
        }
        UpdateWall(mid);
    }

    void UpdateWall((int, int) mid)
    {
        GameObject midWall = GetBoard(mid.Item1, mid.Item2);
        (int, int)[] neighbors = { (0, 1), (1, 0), (0, -1), (-1, 0) };
        List<(int, int)> walls = new List<(int, int)>();
        foreach ((int, int) diff in neighbors)
        {
            (int, int) coord = (mid.Item1 + diff.Item1, mid.Item2 + diff.Item2);
            var obj = GetBoard(coord.Item1, coord.Item2);
            if (obj != null && obj.tag == "Wall")
            {
                walls.Add(diff);
            }
        }

        if (walls.Count == 0)
        {
            UpdateWallTexture(midWall, NoConnections, (0, 1));
        }
        else if (walls.Count == 1)
        {
            UpdateWallTexture(midWall, NoConnections, (0, 1));
        }
        else if (walls.Count == 2)
        {
            if (walls[0].Item1 == -walls[1].Item1 && walls[0].Item2 == -walls[1].Item2)
            {
                UpdateWallTexture(midWall, TwoConnections, walls[0]);
            }
            else
            {
                if (walls[0] == (0, 1) && walls[1] == (-1, 0))
                {
                    UpdateWallTexture(midWall, Corner, walls[1]);
                }
                else
                {
                    UpdateWallTexture(midWall, Corner, walls[0]);
                }
            }
        }
        else if (walls.Count == 3)
        {
            foreach (var neighbor in neighbors)
            {
                if (!walls.Contains(neighbor))
                {
                    UpdateWallTexture(midWall, ThreeConnections, neighbor);
                    midWall.transform.Rotate(0f, -90f, 0f);
                }
            }
        }
        else
        {
            UpdateWallTexture(midWall, FourConnections, (0, 1));
        }

        foreach ((int, int) wall in walls)
        {
            var connectPos = new Vector3(midWall.transform.position.x + wall.Item1 * 2.5f, midWall.transform.position.y, midWall.transform.position.z + wall.Item2 * 2.5f);
            Quaternion direction = new Quaternion();
            switch (wall)
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
            Instantiate(ConnectWallPrefab, connectPos, direction);
        }
    }

    void UpdateWallTexture(GameObject wall, Mesh mesh, (int, int) direction)
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
}
