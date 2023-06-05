using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public int inventorySlotID;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0 )
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            if (draggableItem != null) draggableItem.parentAfterDrag = transform;          
        }
    }
}
