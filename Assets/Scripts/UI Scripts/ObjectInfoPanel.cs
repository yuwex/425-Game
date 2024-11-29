using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;


public class ObjectInfoPanel : MonoBehaviour
{
    public TMP_Text title;
    public GameObject panel;

    [Header("Tower Panel")]
    public GameObject TowerPanel;
    public GameObject statsBoxContainer;
    public GameObject statBoxTemplate;
    public GameObject upgradesBoxContainer;
    public GameObject upgradeBoxTemplate;
    public List<Stat> statsToDisplay;
    public Sprite openSlot;
    // lock icon by aji nugroho
    public Sprite lockedSlot;

    public void SelectGameObject(GameObject gameObject)
    {
        panel.SetActive(gameObject != null);

        if (!gameObject) return;

        if (gameObject.TryGetComponent<Tower>(out var tower))
        {
            SelectTower(tower);
        }

        else
        {
            TowerPanel.SetActive(false);
            panel.SetActive(false);
        }
    }

    public void SelectTower(Tower tower)
    {

        title.text = "Tower";

        TowerPanel.SetActive(true);    
        // Clear both containers
        ClearChildrenWhere(statsBoxContainer, x => x.activeSelf);
        ClearChildrenWhere(upgradesBoxContainer, x => x.activeSelf);

        foreach (Stat stat in statsToDisplay)
        {
            tower.GetStat(stat, out float res);
            GameObject statBox = Instantiate(statBoxTemplate, statsBoxContainer.transform);
            TMP_Text[] texts = statBox.GetComponentsInChildren<TMP_Text>();

            foreach (TMP_Text t in texts)
            {
                switch (t.name)
                {
                    case "Stat":
                        t.text = stat.GetDescription();
                        break;
                    case "Value":
                        t.text = res.ToString("N0");

                        if (stat == Stat.ProjectileLifetime || stat == Stat.TowerCooldown)
                        t.text += "s";

                        break;
                    default:
                        break;
                }
            }

            statBox.SetActive(true);
        }

        
        for (int i = 0; i < tower.maxModifierSlots; i ++)
        {
            GameObject upgradeBox = Instantiate(upgradeBoxTemplate, upgradesBoxContainer.transform);
            UnityEngine.UI.Image[] images = upgradeBox.GetComponentsInChildren<UnityEngine.UI.Image>();
            UnityEngine.UI.Image image = null;

            // Find first image of child
            foreach (UnityEngine.UI.Image im in images)
            {
                if (im != upgradeBox.GetComponent<UnityEngine.UI.Image>())
                {
                    image = im;
                    break;
                }
            }

            image.sprite = (i < tower.unlockedModifierSlots) ? openSlot : lockedSlot;

            if (tower.modifiers.Count > i)
            {
                image.sprite = null;
                image.material = tower.modifiers[i].modifierMaterial;
            }

            upgradeBox.SetActive(true);
        }



    }

    public delegate bool delFilter(GameObject gameObject);

    public void ClearChildrenWhere(GameObject gameObject, delFilter filter)
    {
        List<GameObject> toDelete = new();

        foreach (Transform child in gameObject.transform)
        {
            if (filter(child.gameObject))
            {
                Destroy(child.gameObject);
            }
        }
    }

}
