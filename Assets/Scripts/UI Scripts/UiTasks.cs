using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiTasks : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject ui;
    public GameObject pauseMenu;
    private CursorLockMode originalLockState;
    private bool isPaused = false;


    public void Start()
    {
        originalLockState = Cursor.lockState;
    }
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
        ui.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainHub");
    }

    public void pauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ui.SetActive(false);
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void resumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = originalLockState;
        ui.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }
}
