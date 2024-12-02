using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiTasks : MonoBehaviour
{
    public GameObject player;
    public GameObject gameOverUI;
    public GameObject ui;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject statsMenu;
    public GameObject main;
    public GameObject mainPauseOption;
    public AudioClip UISound;
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
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        Time.timeScale = 1f;
        ui.GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainMenu()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainHub");
    }

    public void pauseGame()
    {
        if (main == null || !main.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ui.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            isPaused = true;
        }
    }

    public void resumeGame()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ui.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    public void optionMenu()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void backButton()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void quitGame()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        // Editor only will remove once game is built
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    public void startGame()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        main.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void viewStats()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        main.SetActive(false);
        statsMenu.SetActive(true);
    }

    public void mainOptions()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        main.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void mainMenuBack()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        statsMenu.SetActive(false);
        main.SetActive(true);
    }

    public void mainOptionsBack()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        optionsMenu.SetActive(false);
        main.SetActive(true);
    }

    public void mainPauseOptions()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        pauseMenu.SetActive(false);
        mainPauseOption.SetActive(true);
    }

    public void menuPauseOptionsBack()
    {
        SoundManager.Instance.PlaySFXClip(UISound, player.transform);
        mainPauseOption.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
