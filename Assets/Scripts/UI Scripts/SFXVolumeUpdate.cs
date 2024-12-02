using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SFXVolumeUpdate : MonoBehaviour
{
    public TMP_Text SFXText;
    public Slider SFXSlider;
    public SoundManager soundManager;

    private void Start()
    {
        SFXSlider.value = GameManager.SFXVolume;
        UpdateSFXText(SFXSlider.value);

        // Add listener to the slider for changes
        SFXSlider.onValueChanged.AddListener(UpdateSFXText);
    }

    public void UpdateSFXText(float SFXval)
    {
        SFXText.text = SFXval.ToString();
        GameManager.SFXVolume = SFXval;
        SoundManager.Instance.volumeSFX = SFXval;
    }
}
