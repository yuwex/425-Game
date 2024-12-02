using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class UpgradeCrossbow : UpgradeStageManager
{

    public WeaponCharger crossbow;

    private Image panelImage;
    private int currStage = 0;
    
    private void Start() {
        // retrieve image component and set color
        panelImage = panel.GetComponent<Image>();
        panelImage.color = upgradeStages[currStage].color;

        // set soul price
        damageText.text = crossbow.totalDamage + "-->"
            + (crossbow.totalDamage + upgradeStages[currStage].damageIncrease);

        // set price button
        buttonText.text = upgradeStages[currStage].price + " Souls";
    }

    public void CanUpgrade()
    {
        if (GameManager.Instance.playerSouls >= upgradeStages[currStage].price && currStage < 3)
        {   
            GameManager.Instance.playerSouls -= upgradeStages[currStage].price;
            UpgradeStage();
        }
        else
        {
            // Button button = upgradeButton.GetComponent<Button>();
            
            // ColorBlock cb = button.colors;
            // cb.pressedColor = Color.red;
            // button.colors = cb;
        }
    }

    private void UpgradeStage() 
    {
        switch (++currStage) 
        {
            case 1:
                // upgrade damage from stage 0 to stage 1
                crossbow.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;

                // calculate current and future damage
                int currDmg = crossbow.maxDamage + crossbow.upgradedDamage;
                int futureDmg = crossbow.maxDamage + crossbow.upgradedDamage + upgradeStages[currStage].damageIncrease;

                // update ui
                panelImage.color = upgradeStages[currStage].color;
                damageText.text = currDmg + "-->" + futureDmg;
                buttonText.text = upgradeStages[currStage].price + " Souls";
                break;

            case 2:
                // upgrade damage from stage 1 to 2
                crossbow.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;

                // calculate current and future damage
                currDmg = crossbow.maxDamage + crossbow.upgradedDamage;
                futureDmg = crossbow.maxDamage + crossbow.upgradedDamage + upgradeStages[currStage].damageIncrease;

                // update ui
                panelImage.color = upgradeStages[currStage].color;
                damageText.text = currDmg + "-->" + futureDmg;
                buttonText.text = upgradeStages[currStage].price + " Souls";
                break;

            case 3:
                // upgrade damage from stage 2 to 3
                crossbow.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;

                buttonText.text = "Max Level!";
                break;
        }


    }
}