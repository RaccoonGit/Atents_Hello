using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    #region Components
    private AudioSource source;
    private AudioClip fireSfx;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    void Awake()
    {
        source = GetComponent<AudioSource>();
        fireSfx = Resources.Load<AudioClip>("Sound/p_m4_SFX");
    }

    void Update()
    {
        
    }
    #endregion
}
