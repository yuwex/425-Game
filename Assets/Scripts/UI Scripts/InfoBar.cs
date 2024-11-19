using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InfoBar : MonoBehaviour
{

    public bool mainGui = true;
    public GameObject barContainer;
    private Slider slider;
    private TMP_Text text; 

    private float value, maxValue;

    void Awake() {
        slider = barContainer.GetComponentInChildren<Slider>(true);
        text = barContainer.GetComponentInChildren<TMP_Text>(true);
        value = slider.value;
        maxValue = slider.maxValue;
    }

    void Update() {
        if (!mainGui) {
            barContainer.transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void SetValue(int value) {
        slider.value = value;
        text.text = (int)value + " / " + (int)maxValue;
    }

    public void SetMaxValue(int v) {
        slider.maxValue = v;
        maxValue = v;
        text.text = (int)value + " / " + (int)maxValue;
    }
}
