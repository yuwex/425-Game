
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title;
    public string message;
    public int priority;
    
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        TooltipManager.Instance.ShowTooltip(title, message, priority);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        TooltipManager.Instance.HideTooltip(priority);
    }

}
