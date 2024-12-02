using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text victoryText, victoryTextBackground;

    private string[] victoryPhrases = {
        "Defenders of the Realm: Victory!",
        "The Waves Have Been Stopped!",
        "The Home Base is Safe, for Now!",
        "The Battle is Won!",
        "You Defended the Fortress!"
    };

    public void displayNewText()
    {
        int randomIndex = Random.Range(0, victoryPhrases.Length);
        victoryText.text = victoryPhrases[randomIndex];  // Set the text of the main victory text
        victoryTextBackground.text = victoryPhrases[randomIndex];  // Set the text of the background as well
    }
}
