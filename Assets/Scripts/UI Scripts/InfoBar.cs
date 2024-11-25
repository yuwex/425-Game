using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InfoBar : MonoBehaviour
{

    public bool faceCamera = false;
    public GameObject barContainer;
    private Slider slider;
    private TMP_Text text; 

    public bool easing = true;

    private float value, targetValue, maxValue;

    void Awake() {
        slider = barContainer.GetComponentInChildren<Slider>(true);
        text = barContainer.GetComponentInChildren<TMP_Text>(true);
        targetValue = slider.value;
        maxValue = slider.maxValue;
    }

    void Update() {
        if (faceCamera) 
        {
            barContainer.transform.rotation = Camera.main.transform.rotation;
        }

        if (easing)
        {
            value += (targetValue - value) * Time.deltaTime * 5;
        }
        else
        {
            value = targetValue;
        }

        slider.value = value;
        text.text = Math.Round(value) + " / " + (int)maxValue;

    }

    public void SetValue(int value) {
        targetValue = value;
    }

    public void SetMaxValue(int v) {
        slider.maxValue = v;
        maxValue = v;
    }
}
