using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    #region Components
    private Rigidbody rbody;
    private TrailRenderer trail;
    #endregion

    #region Public Field
    public float speed = 100.0f;
    public float damage = 25.0f;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    void Awake()
    {
        damage = GameManager.inst.gameData.damage;
        rbody = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();

        Invoke("DeActive", 3.0f);
    }

    void Update()
    {
        rbody.AddForce(transform.forward * speed);
    }

    private void OnEnable()
    {
        rbody.AddForce(transform.forward * speed);
    }

    private void OnDisable()
    {
        trail.Clear();
    }
    #endregion

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
