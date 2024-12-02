using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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

    [Header("UI")]
    public ObjectInfoPanel infoPanel;
    private GameObject objectDisplay;
    public GameObject tileSelectIndicator;

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

        // Enable or disable tower indicator
        if (objectDisplay)
        {
            Vector3 newLoc = board.NormalizePos(objectDisplay.transform.position);
            newLoc.y = 10f + Mathf.Sin(Time.time) * 0.5f;
            tileSelectIndicator.transform.position = newLoc;
            tileSelectIndicator.SetActive(true);
        }
        else
        {
            tileSelectIndicator.SetActive(false);
        }

        if (!buildEnabled) return;

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

        Ray ray = buildCamera.ScreenPointToRay(Input.mousePosition);

        canPlace = GameManager.Instance.playerCoins >= currPrice;
        canPlace = canPlace && !player.bounds.Intersects(renderer.bounds);

        if (!Physics.Raycast(ray, out RaycastHit hit) || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector3 pos = board.NormalizePos(hit.point);

        Collider[] nearby = Physics.OverlapSphere(pos, 2.5f);
        foreach (Collider collider in nearby)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                canPlace = false;
                break;
            }
        }

        var (towerX, towerY) = board.GetCoordinateFromPos(pos);

        GameObject tileObject = board.GetBoard(towerX, towerY);

        if (!tileObject && towerX < board.BoardLength && towerY < board.BoardLength && towerX >= 0 && towerY >= 0)
        {
            HandlePlaceTower(towerX, towerY, pos, renderer);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            objectDisplay = tileObject == objectDisplay ? null : tileObject;
            infoPanel.SelectGameObject(objectDisplay);
        }
    }

    private void HandlePlaceTower(int towerX, int towerY, Vector3 pos, Renderer renderer)
    {
        if (board.GetBaseCoords().Contains((towerX, towerY))) return;

        TowerIndicator.SetActive(true);
        TowerIndicator.transform.position = pos + new Vector3(0, 0.1f, 0);

        bool validPath = PathToBase(pos);

        Collider[] colliders = Physics.OverlapSphere(pos, 0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider is MeshCollider)
            {
                validPath = false;
                break;
            }
        }

        if (validPath && canPlace)
        {
            renderer.material = IndicatorMaterialCanPlace;
        }
        else
        {
            renderer.material = IndicatorMaterialCantPlace;
        }

        if (Input.GetMouseButton(0) && canPlace && validPath)
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