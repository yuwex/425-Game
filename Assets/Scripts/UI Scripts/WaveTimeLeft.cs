using System.Collections;
using TMPro;
using UnityEngine;

public class WaveTimeLeft : MonoBehaviour
{
    public TMP_Text text;
    private bool counting;

    // Start is called before the first frame update
    void Start()
    {
        counting = false;
        text.text = "";
    }

    public void StartCountdown(int time)
    {
        counting = true;
        StartCoroutine(Countdown(time));
    }

    IEnumerator Countdown(int time)
    {
        for (int count = time; count > 0; count--)
        {
            text.text = "" + count;
            yield return new WaitForSeconds(1);
        }
        counting = false;
        text.text = "0";
        yield return new WaitForSeconds(1);
        if (!counting) text.text = "";
    }
}
