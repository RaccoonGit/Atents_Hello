using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Transform itemTr;
    [SerializeField]
    private Transform inventoryTr;
    [SerializeField]
    private Transform itemListTr;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    public static GameObject draggingItem = null;


    void Start()
    {
        itemTr = GetComponent<Transform>();
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryTr = GameObject.Find("Panel-Inventory").transform;
        itemListTr = GameObject.Find("Image-ItemList").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemTr.SetParent(inventoryTr);
        draggingItem = gameObject;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;
        if(itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr.transform);
        }
    }
}
