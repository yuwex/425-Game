using UnityEngine;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject menuCamera;
    public GameObject hubUI;
    private bool mainMenuRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        menuCamera.SetActive(true);
        playerCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseGame();
        }
    }

    public void playGame()
    {
        playerCamera.SetActive(!playerCamera.activeSelf);
        menuCamera.SetActive(!menuCamera.activeSelf);
        hubUI.SetActive(!hubUI.activeSelf);
        mainMenuRunning = false;
    }

    public void pauseGame()
    {
        if (!mainMenuRunning)
        {
            playerCamera.SetActive(!playerCamera.activeSelf);
            menuCamera.SetActive(!menuCamera.activeSelf);
        }
    }
}
