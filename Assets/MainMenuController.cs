using UnityEngine;
using UnityEngine.Events;

public class MainMenuController : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject menuCamera;

    // Start is called before the first frame update
    void Start()
    {
        menuCamera.SetActive(true);
        playerCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playGame()
    {
        playerCamera.SetActive(!playerCamera.activeSelf);
        menuCamera.SetActive(!menuCamera.activeSelf);
    }
}
