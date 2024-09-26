using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    public GameObject TowerPlaceHolder;
    public bool buildEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        TowerPlaceHolder.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && buildEnabled)
        {
            print("here");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(TowerPlaceHolder, hit.point, Quaternion.identity);
            }
            
        }
    }
}
