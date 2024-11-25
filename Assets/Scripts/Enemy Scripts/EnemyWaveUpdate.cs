using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.UI;
using UnityEngine;

public class EnemyWaveUpdate : MonoBehaviour
{
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "" + GameManager.Instance.enemyWave;
    }
}
