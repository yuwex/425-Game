using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildModeButton : MonoBehaviour
{

    public Button button;
    public TowerSpawner towerSpawner;
    private bool buildEnabled = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            ToggleBuildMode();
        }
    }

    public void ToggleBuildMode() {
        buildEnabled = !buildEnabled;
        towerSpawner.buildEnabled = buildEnabled;

        if (buildEnabled) {
            button.image.color = Color.green;
        } else {
            button.image.color = Color.red;
        }

        TMP_Text text = button.GetComponentInChildren<TMP_Text>(true);
        Debug.Log(text);
        if (text != null) {
            if (buildEnabled) {
                text.text = "ON";
            } else {
                text.text = "OFF";
            }
        }

    }

}