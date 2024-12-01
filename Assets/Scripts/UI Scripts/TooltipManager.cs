using System;
using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltipContainer;
    public RectTransform subtextBox;
    public TMP_Text subtextText;
    public RectTransform titleBox;
    public TMP_Text titleText;
    public Camera UICamera;
    public int padding = 10;

    public static TooltipManager Instance;
    private int _priority = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        _priority = 0;
        HideTooltip();
    }

    public void ShowTooltip(string title, string message, int priority)
    {
        if (priority < _priority) return;
        _priority = priority;

        tooltipContainer.SetActive(true);

        subtextText.text = message;
        subtextText.ForceMeshUpdate();

        Vector2 subtextBoxSize = new(subtextText.renderedWidth, subtextText.renderedHeight);
        subtextBoxSize += new Vector2(padding * 2, padding * 2);

        subtextBox.sizeDelta = subtextBoxSize;

        // subtextBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, subtextBoxSize.x);
        // subtextBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, subtextBoxSize.y);
        subtextBox.ForceUpdateRectTransforms();

        titleText.text = title;
        titleText.ForceMeshUpdate();

        Vector2 titleBoxSize = new(titleText.renderedWidth, titleText.renderedHeight);
        titleBoxSize += new Vector2(padding * 2, padding * 2);

        titleBox.sizeDelta = titleBoxSize;
        titleBox.localPosition = Vector3.up * subtextBoxSize.y;
    }

    public void HideTooltip(int priority = int.MaxValue)
    {
        if (priority < _priority) return;
        _priority = 0;

        tooltipContainer.SetActive(false);
    }

    void Update()
    {
        var pos = UICamera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = transform.position.z;

        tooltipContainer.transform.position = pos;
    }
}
