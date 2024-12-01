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

    [Header("Modifier Inventory")]
    public PlayerInventory inventory;
    public GameObject modifierInventoryPanel;
    public RectTransform inventoryBoxContainer;
    public GameObject invBoxTemplate;

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
            DisplayTowerUI(tower);
        }

        else
        {
            TowerPanel.SetActive(false);
            panel.SetActive(false);
        }
    }

    public void DisplayTowerUI(Tower tower)
    {
        DisplayModifiersUI();
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
            
            var image = FindFirstComponentOfChild<UnityEngine.UI.Image>(upgradeBox);
            Tooltip tooltip = upgradeBox.GetComponent<Tooltip>();

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
                ApplyModifierToGameObject(tower.modifiers[i], image, tooltip, upgradeBox.GetComponent<ModifierHolder>());
            }

            upgradeBox.SetActive(true);
        }
    }

    public void DisplayModifiersUI()
    {
        var modifiers = inventory.inventory;

        ClearChildrenWhere(inventoryBoxContainer.gameObject, x => x.activeSelf);
        inventoryBoxContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 90 * modifiers.Count);
        inventoryBoxContainer.ForceUpdateRectTransforms();

        foreach (ModifierBase mod in modifiers)
        {
            GameObject upgradeBox = Instantiate(invBoxTemplate, inventoryBoxContainer.transform);
            var image = FindFirstComponentOfChild<UnityEngine.UI.Image>(upgradeBox);
            var tooltip = upgradeBox.GetComponent<Tooltip>();
            var holder = upgradeBox.GetComponent<ModifierHolder>();

            ApplyModifierToGameObject(mod, image, tooltip, holder);
            upgradeBox.SetActive(true);
        }
    }

    private T FindFirstComponentOfChild<T>(GameObject g)
    {
            T[] childComps = g.GetComponentsInChildren<T>();
            T parentComp = g.GetComponent<T>();

            // Find first image of child
            foreach (T child in childComps)
            {
                if (!child.Equals(parentComp))
                {
                    return child;
                }
            }

            return default;
    }

    private void ApplyModifierToGameObject(ModifierBase modifier, UnityEngine.UI.Image image, Tooltip tooltip, ModifierHolder holder)
    {
        image.sprite = null;
        image.material = modifier.modifierMaterial;
        holder.modifier = modifier;
        tooltip.title = modifier.modifierName;
        tooltip.message = modifier.description;
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
