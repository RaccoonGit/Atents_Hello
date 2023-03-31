using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    private Transform slotTr;

    private void Start()
    {
        slotTr = GetComponent<Transform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            Drag.draggingItem.transform.parent = slotTr;
        }
    }
}
