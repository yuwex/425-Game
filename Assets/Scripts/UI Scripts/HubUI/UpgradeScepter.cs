using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class UpgradeScepter : UpgradeStageManager
{

    public WeaponProjectile scepter;

    private Image panelImage;
    private int currStage = 0;

    [Header("SFX")]
    public AudioClip clickSound;

    private void Start() {
        // retrieve image component and set color
        panelImage = panel.GetComponent<Image>();
        panelImage.color = upgradeStages[currStage].color;

        // set soul price
        damageText.text = scepter.totalDamage + "-->"
            + (scepter.totalDamage + upgradeStages[currStage].damageIncrease);

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
        SoundManager.Instance.PlaySFXClip(clickSound, transform);

        switch (++currStage) 
        {
            case 1:
                // upgrade damage from stage 0 to stage 1
                scepter.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;

                // calculate current and future damage
                int currDmg = scepter.attackDamage + scepter.upgradedDamage;
                int futureDmg = scepter.attackDamage + scepter.upgradedDamage + upgradeStages[currStage].damageIncrease;

                // update ui
                panelImage.color = upgradeStages[currStage].color;
                damageText.text = currDmg + "-->" + futureDmg;
                buttonText.text = upgradeStages[currStage].price + " Souls";
                break;

            case 2:
                // upgrade damage from stage 1 to 2
                scepter.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;

                // calculate current and future damage
                currDmg = scepter.attackDamage + scepter.upgradedDamage;
                futureDmg = scepter.attackDamage + scepter.upgradedDamage + upgradeStages[currStage].damageIncrease;

                // update ui
                panelImage.color = upgradeStages[currStage].color;
                damageText.text = currDmg + "-->" + futureDmg;
                buttonText.text = upgradeStages[currStage].price + " Souls";
                break;

            case 3:
                // upgrade damage from stage 2 to 3
                scepter.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;

                buttonText.text = "Max Level!";
                break;
        }


    }
}
