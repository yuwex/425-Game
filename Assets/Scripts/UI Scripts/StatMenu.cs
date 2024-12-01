using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatMenu : MonoBehaviour
{
    public TMP_Text enemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        enemiesKilled.text = GameManager.enemiesKilled.ToString();
    }
}
