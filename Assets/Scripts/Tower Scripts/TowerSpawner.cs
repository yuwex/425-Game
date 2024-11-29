using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("PathFinding")]
    public Transform homeBase;
    public List<Transform> spawnPoints;
    public CoinWarning coinWarning;

    // Start is called before the first frame update
    void Start()
    {
        TowerPlaceHolder.transform.localScale = towerScale;

        // Set options for the indicator
        TowerIndicator = Instantiate(TowerPlaceHolder, Vector3.zero, Quaternion.identity);
        TowerIndicator.GetComponent<MeshCollider>().enabled = false;
        TowerIndicator.GetComponent<NavMeshObstacle>().enabled = false;
        TowerIndicator.GetComponent<MeshRenderer>().material = IndicatorMaterialCanPlace;

        currObject = TowerPlaceHolder;
        currPrice = towerPrice;

        mesh = TowerIndicator.GetComponent<MeshFilter>();
        canPlace = true;
    }

    // Update is called once per frame
    void Update()
    {
        TowerIndicator.SetActive(false);

        // Update tower material depending whether or not player has enough coins
        var renderer = TowerIndicator.GetComponent<MeshRenderer>();

        if (Input.GetKeyDown(KeyCode.Alpha1) && currObject != TowerPlaceHolder)
        {
            mesh.sharedMesh = TowerPlaceHolder.GetComponent<MeshFilter>().sharedMesh;
            mesh.transform.localScale = towerScale;
            currPrice = towerPrice;
            currObject = TowerPlaceHolder;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && currObject != WallPlaceHolder)
        {
            mesh.sharedMesh = WallPlaceHolder.GetComponent<MeshFilter>().sharedMesh;
            mesh.transform.localScale = wallScale;
            currPrice = wallPrice;
            currObject = WallPlaceHolder;
        }
        if (buildEnabled)
        {
            Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            canPlace = GameManager.Instance.playerCoins >= currPrice;
            canPlace = canPlace && !player.bounds.Intersects(renderer.bounds);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 pos = board.NormalizePos(hit.point);
                var (towerX, towerY) = board.GetCoordinateFromPos(pos);

                if (board.GetBoard(towerX, towerY) == null && !board.GetBaseCoords().Contains((towerX, towerY)))
                {
                    TowerIndicator.SetActive(true);
                    TowerIndicator.transform.position = pos + new Vector3(0, 0.1f, 0);

                    bool validPath = PathToBase(pos);

                    if (validPath && canPlace)
                    {
                        renderer.material = IndicatorMaterialCanPlace;
                    }
                    else
                    {
                        renderer.material = IndicatorMaterialCantPlace;
                    }


                    if (Input.GetMouseButtonDown(0) && canPlace && validPath)
                    {
                        var tower = Instantiate(currObject, pos, Quaternion.identity);
                        board.SetBoard(towerX, towerY, tower);

                        board.UpdateWalls(pos);

                        // Subtract tower price from user coins
                        GameManager.Instance.updateCoins(-currPrice);
                    }
                    else if (Input.GetMouseButtonDown(0) && !canPlace)
                    {
                        coinWarning.ShowWarning();
                    }
                }
            }
        }
    }


    private bool PathToBase(Vector3 towerPosition)
    {
        var (towerX, towerY) = board.GetCoordinateFromPos(towerPosition);
        var temp = new GameObject("Temp");
        board.SetBoard(towerX, towerY, temp);

        var (homeX, homeY) = board.GetCoordinateFromPos(homeBase.position);

        foreach (Transform spawn in spawnPoints)
        {
            var (startX, startY) = board.GetCoordinateFromPos(spawn.position);
            if (board.PathExists(startX, startY, homeX, homeY))
            {
                board.SetBoard(towerX, towerY, null);
                Destroy(temp);
                return true;
            }
        }

        board.SetBoard(towerX, towerY, null);
        Destroy(temp);
        return false;
    }
}