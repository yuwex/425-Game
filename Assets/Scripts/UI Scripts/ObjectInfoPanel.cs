using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SelectedPanelType
{
    None,
    Tower,
}

public class ObjectInfoPanel : MonoBehaviour
{
    public TMP_Text title;
    public GameObject panel;
    public GameObject selected;
    public SelectedPanelType selectedType;

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

    void Start()
    {
        selectedType = SelectedPanelType.None;
        selected = null;
    }

    public void SelectGameObject(GameObject gameObject)
    {
        panel.SetActive(gameObject != null);

        if (!gameObject)
        {
            selected = null;
            selectedType = SelectedPanelType.None;
            return;
        };

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

        selected = tower.gameObject;
        selectedType = SelectedPanelType.Tower;

        title.text = "Tower";

        TowerPanel.SetActive(true);    
        // Clear both containers
        ClearChildrenWhere(statsBoxContainer, x => x.activeSelf);
        ClearChildrenWhere(upgradesBoxContainer, x => x.activeSelf);

        // Reset size of statsbox
        var rt = statsBoxContainer.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90 * (statsToDisplay.Count + 1));
        rt.ForceUpdateRectTransforms();

        foreach (Stat stat in statsToDisplay)
        {
            tower.GetStat(stat, out float res);
            GameObject statBox = Instantiate(statBoxTemplate, statsBoxContainer.transform);
            TMP_Text[] texts = statBox.GetComponentsInChildren<TMP_Text>();
            
            Tooltip tooltip = statBox.GetComponentInChildren<Tooltip>();
            tooltip.title = "";
            tooltip.message = tower.statChangeDescriptions[stat];

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

            Tooltip tooltip = upgradeBox.GetComponent<Tooltip>();

            // Find first image of child
            foreach (UnityEngine.UI.Image im in images)
            {
                if (im != upgradeBox.GetComponent<UnityEngine.UI.Image>())
                {
                    image = im;
                    break;
                }
            }

            if (i < tower.unlockedModifierSlots)
            {
                image.sprite = openSlot;
                tooltip.title = "Unlocked";
                tooltip.message = "Click to apply an upgrade!";
            }
            else
            {
                image.sprite = lockedSlot;
                tooltip.title = "Locked";
                tooltip.message = "Upgrade this tower to unlock this slot!";
            }

            if (tower.modifiers.Count > i)
            {
                image.sprite = null;
                var modifier = tower.modifiers[i];
                image.material = modifier.modifierMaterial;
                upgradeBox.GetComponent<ModifierHolder>().modifier = modifier;

                tooltip.title = $"Upgrade: {modifier.modifierName}";
                tooltip.message = modifier.description;
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
