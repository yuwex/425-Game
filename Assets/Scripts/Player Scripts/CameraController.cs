using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject buildCamera;
    public GameObject pauseCamera;
    bool buildCameraOn = false;
    public UnityEvent onBuildMode;
    public UnityEvent onMainMode;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.SetActive(true);
        buildCamera.SetActive(false);
        pauseCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mainCamera.SetActive(!mainCamera.activeSelf);
            buildCamera.SetActive(!buildCamera.activeSelf);
            buildCameraOn = !buildCameraOn;

            if (buildCameraOn)
            {
                onBuildMode.Invoke();
            }
            else
            {
                onMainMode.Invoke();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!buildCameraOn)
            {
                pauseCamera.SetActive(!pauseCamera.activeSelf);
                mainCamera.SetActive(!mainCamera.activeSelf);
            }
            else
            {
                pauseCamera.SetActive(!pauseCamera.activeSelf);
                buildCamera.SetActive(!buildCamera.activeSelf);
            }
        }
    }

    public void clickUnpause()
    {
        if (!buildCameraOn)
        {
            pauseCamera.SetActive(!pauseCamera.activeSelf);
            mainCamera.SetActive(!mainCamera.activeSelf);
        }
        else
        {
            pauseCamera.SetActive(!pauseCamera.activeSelf);
            buildCamera.SetActive(!buildCamera.activeSelf);
        }
    }
}
