using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePosCtrl : MonoBehaviour
{
    private float damping = 10.0f;
    public void SetFirePosRotation(Quaternion rot)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
    }
}
