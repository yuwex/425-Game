using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class MusicVolume : MonoBehaviour
{
    public TMP_Text MusicText;
    public Slider MusicSlider;
    public SoundManager soundManager;

    private void Start()
    {
        MusicSlider.value = GameManager.MusicVolume;
        UpdateSFXText(MusicSlider.value);

        // Add listener to the slider for changes
        MusicSlider.onValueChanged.AddListener(UpdateSFXText);
    }

    public void UpdateSFXText(float MusicVal)
    {
        MusicText.text = MusicVal.ToString();
        GameManager.MusicVolume = MusicVal;
        SoundManager.Instance.volumeMusic = MusicVal;
    }
}
