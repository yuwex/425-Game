using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [Header("Placeholders")]
    public GameObject TowerPlaceHolder;
    public GameObject WallPlaceHolder;

    [Header("Materials")]
    public Material IndicatorMaterialCanPlace;
    public Material IndicatorMaterialCantPlace;

    [Header("Tower Settings")]
    public Vector3 towerScale;
    public int towerPrice;

    [Header("Wall Settings")]
    public Vector3 wallScale;
    public int wallPrice;

    [Header("Other Dependencies")]
    public GameBoard board;
    public Camera buildCamera;
    public bool buildEnabled = false;
    public CharacterController player;

    private GameObject TowerIndicator;
    private MeshFilter mesh;

    private GameObject currObject;
    private int currPrice;

    private bool canPlace;


    // Start is called before the first frame update
    void Start()
    {
        TowerPlaceHolder.transform.localScale = towerScale;

        // Set options for the indicator
        TowerIndicator = Instantiate(TowerPlaceHolder, Vector3.zero, Quaternion.identity);
        TowerIndicator.GetComponent<MeshCollider>().enabled = false;
        TowerIndicator.GetComponent<Tower>().enabled = false;

        mesh = TowerIndicator.GetComponent<MeshFilter>();

        currObject = TowerPlaceHolder;
        currPrice = towerPrice;

        var renderer = TowerIndicator.GetComponent<MeshRenderer>();
        renderer.material = IndicatorMaterialCanPlace;

        canPlace = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Update tower material depending whether or not player has enough coins
        var renderer = TowerIndicator.GetComponent<MeshRenderer>();


        TowerIndicator.SetActive(false);
        if (buildEnabled)
        {
            Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 pos;

            canPlace = GameManager.Instance.playerCoins >= currPrice;
            canPlace = canPlace && !player.bounds.Intersects(renderer.bounds);

            if (Physics.Raycast(ray, out hit))
            {

                pos = board.NormalizePos(hit.point);
                if (board.GetObjectFromPos(pos) is null)
                {

                    TowerIndicator.SetActive(true);
                    TowerIndicator.transform.position = pos + new Vector3(0, 0.1f, 0);

                    if (canPlace)
                    {
                        renderer.material = IndicatorMaterialCanPlace;
                    }
                    else
                    {
                        renderer.material = IndicatorMaterialCantPlace;
                    }

                    if (Input.GetMouseButtonDown(0) && canPlace)
                    {
                        var tower = Instantiate(currObject, pos, Quaternion.identity);
                        board.SetObjectAtPos(pos, tower);

                        // Subtract tower price from user coins
                        GameManager.Instance.updateCoins(-currPrice);

                        board.UpdateWalls(pos);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && currObject != WallPlaceHolder)
        {
            mesh.sharedMesh = WallPlaceHolder.GetComponent<MeshFilter>().sharedMesh;
            mesh.transform.localScale = wallScale;
            currPrice = wallPrice;
            currObject = WallPlaceHolder;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && currObject != TowerPlaceHolder)
        {
            mesh.sharedMesh = TowerPlaceHolder.GetComponent<MeshFilter>().sharedMesh;
            mesh.transform.localScale = towerScale;
            currPrice = towerPrice;
            currObject = TowerPlaceHolder;
        }
    }
}
