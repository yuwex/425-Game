using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiTasks : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject ui;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject statsMenu;
    public GameObject main;
    public GameObject mainPauseOption;
    // private CursorLockMode originalLockState;
    private bool isPaused = false;


    // public void Start()
    // {
    //     originalLockState = Cursor.lockState;
    // }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseGame();
            }
            else
            {
                resumeGame();
            }
        }
    }
    public void gameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        ui.SetActive(false);
    }
    public void restartLevel()
    {
        Time.timeScale = 1f;
        ui.GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainHub");
    }

    public void pauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ui.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void resumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ui.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void optionMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void backButton()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void quitGame()
    {
        // Editor only will remove once game is built
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    public void startGame()
    {
        main.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void viewStats()
    {
        main.SetActive(false);
        statsMenu.SetActive(true);
    }

    public void mainOptions()
    {
        main.SetActive(false);
        optionsMenu .SetActive(true);
    }

    public void mainMenuBack()
    {
        statsMenu.SetActive(false);
        main.SetActive(true);
    }

    public void mainOptionsBack()
    {
        optionsMenu.SetActive(false);
        main.SetActive(true);
    }

    public void mainPauseOptions()
    {
        pauseMenu.SetActive(false);
        mainPauseOption.SetActive(true);
    }

    public void menuPauseOptionsBack()
    {
        mainPauseOption.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
