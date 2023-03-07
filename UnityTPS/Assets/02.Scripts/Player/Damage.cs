using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private string e_bulletTag = "E_BULLET";
    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == e_bulletTag)
        {
            Destroy(col.gameObject);
        }
    }
}
