using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject buildCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.SetActive(true);
        buildCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mainCamera.SetActive(mainCamera.gameObject.activeSelf);
            buildCamera.SetActive(!buildCamera.gameObject.activeSelf);
        }
    }
}
