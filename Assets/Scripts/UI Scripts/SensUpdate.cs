using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SensUpdate : MonoBehaviour
{
    public TMP_Text sensitivityText;
    public Slider sensSlider;
    public MouseControls mouseControls;

    private void Start()
    {
        sensSlider.value = GameManager.sensValue;
        UpdateSensText(sensSlider.value);

        // Add listener to the slider for changes
        sensSlider.onValueChanged.AddListener(UpdateSensText);
    }

    public void UpdateSensText(float sensVal)
    {
        sensitivityText.text = sensVal.ToString();
        GameManager.sensValue = sensVal;
    }
}
