using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SensUpdate : MonoBehaviour
{
    public TMP_Text sensitivityText;
    public Slider sensSlider;

    private void Start()
    {
        UpdateSensText(sensSlider.value);
        sensSlider.onValueChanged.AddListener(UpdateSensText);
    }

    public void UpdateSensText(float sensVal)
    {
        sensitivityText.text = sensVal.ToString();
    }
}
