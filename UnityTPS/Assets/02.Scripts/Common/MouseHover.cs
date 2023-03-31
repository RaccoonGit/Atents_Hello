using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    public bool isHover;

    public static MouseHover mouseHover;
    void Start()
    {
        mouseHover = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }
}
