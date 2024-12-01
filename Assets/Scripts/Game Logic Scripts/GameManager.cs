using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Runtime.CompilerServices;

public class GameManager : MonoBehaviour
{
    public int enemyWave = 0;

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
    }
    /******************** SINGLETON ********************/




    // currency 
    public int playerCoins = 150;
    public void updateCoins(int coins)
    {
        playerCoins += coins;
    }



    /*** TRANSITION HANDLING ***/

    public Animator trans;

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

        // trigger fade-in animation
        trans.SetTrigger("End");

        // wait for 1 second
        yield return new WaitForSeconds(1);

        // load the next scene
        Addressables.LoadSceneAsync(scene, LoadSceneMode.Single);

        // trigger fade-out animation
        trans.SetTrigger("Start");
    }
    /*** TRANSITION HANDLING ***/



    public void quitGame()
    {
        StartCoroutine(LoadScene());
    }

}
