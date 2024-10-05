using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public GameObject TowerPlaceHolder;
    public GameBoard board;
    public bool buildEnabled = false;

    private GameObject TowerIndicator;
    public Material IndicatorMaterialCanBuy;
    public Material IndicatorMaterialCantBuy;
    private bool canPlace;

    // Start is called before the first frame update
    void Start()
    {
        TowerPlaceHolder.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);


        // Set options for the indicator
        TowerIndicator = Instantiate(TowerPlaceHolder, Vector3.zero, Quaternion.identity);
        TowerIndicator.GetComponent<MeshCollider>().enabled = false;
        TowerIndicator.GetComponent<Tower>().enabled = false;

        var renderer = TowerIndicator.GetComponent<MeshRenderer>();
        renderer.material = IndicatorMaterialCanBuy;

        canPlace = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Update tower material depending whether or not player has enough coins
        var renderer = TowerIndicator.GetComponent<MeshRenderer>();
        if (GameManager.Instance.playerCoins >= 100)
        {
            renderer.material = IndicatorMaterialCanBuy;
            canPlace = true;
        } else
        {
            renderer.material = IndicatorMaterialCantBuy;
            canPlace = false;
        }

        TowerIndicator.SetActive(false);
        if (buildEnabled) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 pos;

            if (Physics.Raycast(ray, out hit)) {

                pos = board.NormalizePos(hit.point);
                if (board.GetObjectFromPos(pos) is null) {
                    TowerIndicator.SetActive(true);
                    TowerIndicator.transform.position = pos + new Vector3(0, 0.1f, 0);

                    if (Input.GetMouseButtonDown(0) && canPlace) {
                        var tower = Instantiate(TowerPlaceHolder, pos, Quaternion.identity);
                        board.SetObjectAtPos(pos, tower);

                        // Subtract tower price from user coins
                        GameManager.Instance.updateCoins(-100);
                    }
                }                
            }
        }
    }
}
