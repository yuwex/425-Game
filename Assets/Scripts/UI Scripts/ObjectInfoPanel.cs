using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject towerPanel;
    public GameObject statsBoxContainer;
    public GameObject statBoxTemplate;
    public GameObject upgradesBoxContainer;
    public GameObject upgradeBoxTemplate;
    public List<Stat> statsToDisplay;
    public Sprite openSlot;
    public Sprite lockedSlot;

    [Header("Modifier Inventory")]
    public PlayerInventory inventory;
    public GameObject modifierInventoryPanel;
    public RectTransform inventoryBoxContainer;
    public GameObject invBoxTemplate;

    [Header("SFX")]
    public AudioClip clickSound;

    void Start()
    {
        selectedType = SelectedPanelType.None;
        selected = null;
        GameManager.Instance.onUpdateCoins.AddListener(() => RefreshDisplayedElements());
    }

    public void SelectGameObject(GameObject gameObject)
    {
        SoundManager.Instance.PlaySFXClip(clickSound, transform);

        panel.SetActive(gameObject != null);

        if (!gameObject)
        {
            selected = null;
            selectedType = SelectedPanelType.None;
            return;
        };

        if (gameObject.TryGetComponent<Tower>(out var tower))
        {
            modifierInventoryPanel.SetActive(false);
            towerPanel.SetActive(true);
            DisplayTowerUI(tower);
        }

        else
        {
            towerPanel.SetActive(false);
            modifierInventoryPanel.SetActive(false);
            panel.SetActive(false);
        }
    }

    public void DisplayTowerUI(Tower tower)
    {
        DisplayModifiersUI(tower);
        selected = tower.gameObject;
        selectedType = SelectedPanelType.Tower;

        title.text = "Tower";
    
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
            
            var button = upgradeBox.GetComponent<Button>();
            var image = FindFirstComponentOfChild<UnityEngine.UI.Image>(upgradeBox);
            Tooltip tooltip = upgradeBox.GetComponent<Tooltip>();

            if (tower.modifiers.Count > i)
            {
                var mod = tower.modifiers[i];

                ApplyModifierToGameObject(mod, image, tooltip, upgradeBox.GetComponent<ModifierHolder>());
                button.onClick.AddListener(() => {
                    SoundManager.Instance.PlaySFXClip(clickSound, transform);
                    tower.modifiers.Remove(mod);
                    inventory.inventory.Add(mod);
                    tower.UpdateModifiers();
                    RefreshDisplayedElements();
                    modifierInventoryPanel.SetActive(true);
                });
            }
            else if (i < tower.unlockedModifierSlots)
            {
                image.sprite = openSlot;
                tooltip.title = "Unlocked";
                tooltip.message = "Click to apply an upgrade!";

                if (inventory.inventory.Count == 0)
                {
                    tooltip.message = "Once you find an upgrade,\nclick this button to equip it!";
                }
                else
                {
                    button.onClick.AddListener(() => {
                        SoundManager.Instance.PlaySFXClip(clickSound, transform);
                        modifierInventoryPanel.SetActive(true);
                    });
                }
            }
            else
            {
                image.sprite = lockedSlot;

                var cost = tower.modifierSlotCosts[i];
                tooltip.title = $"Buy for {cost} coins";

                if (i != tower.unlockedModifierSlots)
                {
                    tooltip.message = "Unlock all previous slots to unlock this one";
                }
                else if (cost > GameManager.Instance.playerCoins) 
                {
                    tooltip.message = "You don\'t have enough coins to <color=red>unlock</color> this slot";
                }
                else
                {
                    tooltip.message = "Click to <color=green>unlock</color> this slot";

                    button.onClick.AddListener(() => {
                        SoundManager.Instance.PlaySFXClip(clickSound, transform);
                        GameManager.Instance.updateCoins(-cost);
                        tower.unlockedModifierSlots += 1;
                        RefreshDisplayedElements();
                    });
                }


            }



            upgradeBox.SetActive(true);
        }
    }

    public void DisplayModifiersUI(Tower tower)
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
            var button = upgradeBox.GetComponent<Button>();

            button.onClick.AddListener(() => {
                SoundManager.Instance.PlaySFXClip(clickSound, transform);
                if (tower.unlockedModifierSlots > tower.modifiers.Count)
                {
                    tower.modifiers.Add(mod);
                    inventory.inventory.Remove(mod);
                    tower.UpdateModifiers();
                    DisplayTowerUI(tower);

                    if (tower.unlockedModifierSlots <= tower.modifiers.Count || inventory.inventory.Count == 0)
                        modifierInventoryPanel.SetActive(false);
                    
                    TooltipManager.Instance.HideTooltip();
                }
                else
                {
                    TooltipPopup(
                        "Error",
                        "<color=red>Not enough slots</color>",
                        3
                    );
                }
                
            });

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

    public void TooltipPopup(string title, string message, float time, int priority = 10)
    {
        TooltipManager tooltip = TooltipManager.Instance;

        IEnumerator _TooltipPopup(string title, string message, float time, int priority = 10)
        {
            tooltip.ShowTooltip(title, message, priority);
            yield return new WaitForSeconds(time);
            tooltip.HideTooltip(priority);
        }

        StartCoroutine(_TooltipPopup(title, message, time, priority));
    }

    public void HideTooltip()
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void DisableObjectInfoPanel()
    {
        panel.SetActive(false);
    }

    public void EnableIfSelected()
    {
        if (!selected) return;
        
        panel.SetActive(true);
        RefreshDisplayedElements();
    }

    public void RefreshDisplayedElements()
    {
        if (selectedType == SelectedPanelType.Tower)
        {
            DisplayTowerUI(selected.GetComponent<Tower>());
        }
    }



}
