using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro
using System.Collections;

public class CoinWarning : MonoBehaviour
{
    public TextMeshProUGUI warningText;
    private float fadeDuration = 0.1f;

    void start()
    {
        warningText.enabled = false;
    }
    public void ShowWarning()
    {
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        Color color = warningText.color;
        color.a = 0; //Transparent
        warningText.color = color;
        warningText.enabled = true;

        // Fade in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            warningText.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Fade out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            warningText.color = color;
            yield return null;
        }

        warningText.enabled = false;
    }
}
