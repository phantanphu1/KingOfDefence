using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    public int row, col;
    public bool IsEmpty { get; set; } = true;
    public Character CharacterOnCell { get; set; } = null;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"OnDroop[{row},{col}]");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
