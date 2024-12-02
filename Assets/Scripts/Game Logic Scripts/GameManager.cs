using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int enemyWave = 0;
    public static int enemiesKilled = 0;
    public static float sensValue = 4f;
    public static float SFXVolume = 0.5f;
    public static float MusicVolume = 0.5f;
    /******************** SINGLETON ********************/
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is NULL");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this);

        if (trans) trans.SetActive(false);
    }
    /******************** SINGLETON ********************/

    // currencies
    public int playerCoins = 150;
    public int playerSouls = 10000;

    public UnityEvent onUpdateCoins;

    public void updateCoins(int coins)
    {
        playerCoins += coins;
        onUpdateCoins.Invoke();
    }

    public void updateStats()
    {
        enemiesKilled++;
        playerSouls++;
    }


    /*** TRANSITION HANDLING ***/

    public GameObject trans;

    public void SceneTransition()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {

        string scene;

        // find which scene to choose
        if (SceneManager.GetActiveScene().name == "MainHub")
        {
            scene = "Game";
        }
        else
        {
            scene = "MainHub";
        }

        trans.SetActive(true);

        // trigger fade-in animation
        trans.GetComponent<Animator>().SetTrigger("End");

        // wait for 1 second
        yield return new WaitForSeconds(1);

        // load the next scene
        Addressables.LoadSceneAsync(scene, LoadSceneMode.Single);

        // trigger fade-out animation
        trans.GetComponent<Animator>().SetTrigger("Start");

        yield return new WaitForSeconds(1);

        trans.SetActive(false);
    }
    /*** TRANSITION HANDLING ***/

    public void quitGame()
    {
        StartCoroutine(LoadScene());
    }

}
