using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class UpgradeSword : UpgradeStageManager
{

    public WeaponHitscan sword;

    private Image panelImage;
    private int currStage = 0;

    [Header("SFX")]
    public AudioClip clickSound;
    public AudioClip errSound;
    
    private void Start() {
        // retrieve image component and set color
        panelImage = panel.GetComponent<Image>();
        panelImage.color = upgradeStages[currStage].color;

        // set soul price
        damageText.text = sword.totalDamage + "-->"
            + (sword.totalDamage + upgradeStages[currStage].damageIncrease);

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

            SoundManager.Instance.PlaySFXClip(errSound, transform);
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
                sword.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;
                
                // calculate current and future damage
                int currDmg = sword.attackDamage + sword.upgradedDamage;
                int futureDmg = sword.attackDamage + sword.upgradedDamage + upgradeStages[currStage].damageIncrease;

                // update ui
                panelImage.color = upgradeStages[currStage].color;
                damageText.text = currDmg + "-->" + futureDmg;
                buttonText.text = upgradeStages[currStage].price + " Souls";
                break;

            case 2:
                // upgrade damage from stage 1 to 2
                sword.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;

                // calculate current and future damage
                currDmg = sword.attackDamage + sword.upgradedDamage;
                futureDmg = sword.attackDamage + sword.upgradedDamage + upgradeStages[currStage].damageIncrease;

                // update ui
                panelImage.color = upgradeStages[currStage].color;
                damageText.text = currDmg + "-->" + futureDmg;
                buttonText.text = upgradeStages[currStage].price + " Souls";
                break;

            case 3:
                // upgrade damage from stage 2 to 3
                sword.upgradedDamage += upgradeStages[currStage - 1].damageIncrease;

                buttonText.text = "Max Level!";
                break;
        }


    }
}
