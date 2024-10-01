using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GameBoard : MonoBehaviour
{

    public int BoardLength, TileSize;
    private GameObject[,] Tiles;

    void Awake() {
        Tiles = new GameObject[BoardLength, BoardLength];
    }

    public GameObject GetBoard(int x, int y) {
        return Tiles[x,y];
    }

    public void SetBoard(int x, int y, GameObject g) {
        Tiles[x, y] = g;
    }

    void Update() {
        // if (Input.GetMouseButton(0)) {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit)) {
        //         Debug.Log(NormalizePos(hit.point) + " vs " + hit.point);
                
        //         // Debug.Log("(" + x + ", " + z + ") vs " + hit.point);
                
        //     }
        // }
    }

    public (int, int) GetCoordinateFromPos(Vector3 pos) {
        var offset = BoardLength / 2 * TileSize;

        return (
            (int)Math.Floor((pos.x + offset) / 5),
            (int)Math.Floor((pos.z + offset) / 5)
        );
    }

    public Vector3 NormalizePos(Vector3 pos) {
        var (x, z) = GetCoordinateFromPos(pos);
        float offset = BoardLength / 2f * TileSize - (TileSize / 2f);

        return new Vector3(x * 5 - offset, 0,  z * 5 - offset);
    }

    public GameObject GetObjectFromPos(Vector3 pos) {
        var (x, y) = GetCoordinateFromPos(pos);
        return GetBoard(x, y);
    }

    public void SetObjectAtPos(Vector3 pos, GameObject g) {
        var (x, y) = GetCoordinateFromPos(pos);
        SetBoard(x, y, g);
    }
}
