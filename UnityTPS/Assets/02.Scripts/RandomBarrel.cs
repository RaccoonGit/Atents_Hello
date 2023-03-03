using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBarrel : MonoBehaviour
{
    private MeshRenderer curMat;
    private Material[] list;
    void Start()
    {
        curMat = GetComponent<MeshRenderer>();
        list = GetComponent<MeshRenderer>().materials;

        int ran = Random.Range(0, 4);
        curMat.material = list[ran];
    }
}
