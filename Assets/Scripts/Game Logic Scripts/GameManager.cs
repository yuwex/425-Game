using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

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

    // Currency 
    public int playerCoins;
    public void updateCoins(int coins)
    {
        playerCoins += coins;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerCoins = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
