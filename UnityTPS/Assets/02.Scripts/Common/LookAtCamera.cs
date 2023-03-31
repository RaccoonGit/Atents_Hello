using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public RectTransform CanvasTr;
    public Transform mainCamTr;
    void Start()
    {
        CanvasTr = GetComponent<RectTransform>();
        mainCamTr = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CanvasTr.LookAt(mainCamTr);
    }
}
