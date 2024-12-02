using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class UpgradeStageManager : MonoBehaviour
{
    [System.Serializable]
    public class Stage
    {
        // define members of stage class
        public Color color;
        public int price;
        public int damageIncrease;

        // stage constructor
        public Stage(Color32 color, int price, int damageIncrease)
        {
            this.color = color;
            this.price = price;
            this.damageIncrease = damageIncrease;
        }
    }

    // create list of three stages
    public List<Stage> upgradeStages = new List<Stage>();
    public GameObject panel;
    public GameObject upgradeButton;
    public TMP_Text damageText;
    public TMP_Text buttonText;

    // add stage instances to list
    private void Awake() {
        upgradeStages.Add(new Stage(new Color32(76,81,247,255), 100, 10));
        upgradeStages.Add(new Stage(new Color32(157,77,187,255), 250, 30));
        upgradeStages.Add(new Stage(new Color32(243,175,25,255), 1000, 50));
    }
}
