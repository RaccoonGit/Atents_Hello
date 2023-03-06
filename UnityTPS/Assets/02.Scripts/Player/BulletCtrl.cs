using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    #region Components
    private Rigidbody rbody;
    #endregion

    #region Public Field
    public float speed = 100.0f;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rbody.AddForce(transform.forward * speed);
    }
    #endregion
}
