using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro
using System.Collections;

public class StartRoundText : MonoBehaviour
{
    public TextMeshProUGUI startWaveText;
    public float fadeDuration = 0.1f;
    public GameObject ememySpawner;
    private bool roundStarted;

    void start()
    {
        startWaveText.enabled = false;
        roundStarted = false;
    }

    void Update()
    {
        if (ememySpawner.GetComponent<EnemySpawner>().betweenWaves)
        {
            startWaveText.enabled = true;
            roundStarted = false;
            Color color = startWaveText.color;
            color.a = 1;
            startWaveText.color = color;
        }
        if (!ememySpawner.GetComponent<EnemySpawner>().betweenWaves && !roundStarted)
        {
            WaveStarted();
            roundStarted = true;
        }
    }

    public void WaveStarted()
    {
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        Color color = startWaveText.color;
        color.a = 1;
        startWaveText.color = color;
        startWaveText.enabled = true;

        // Fade out
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            startWaveText.color = color;
            yield return null;
        }

        startWaveText.enabled = false;
    }
}