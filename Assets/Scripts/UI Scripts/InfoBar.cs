using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoBar : MonoBehaviour
{

    public GameObject barContainer;
    private Slider slider;
    private TMP_Text text; 

    private float value, maxValue;

    void Start() {
        slider = barContainer.GetComponentInChildren<Slider>(true);
        text = barContainer.GetComponentInChildren<TMP_Text>(true);

        value = slider.value;
        maxValue = slider.maxValue;
    }
    public void setValue(int value) {
        slider.value = value;
        text.text = (int)value + " / " + (int)maxValue;
    }

    public void setMaxValue(int value) {
        slider.maxValue = value;
        text.text = (int)value + " / " + (int)maxValue;
    }
}
