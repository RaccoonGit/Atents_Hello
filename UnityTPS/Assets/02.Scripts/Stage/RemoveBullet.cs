using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    #region Tag String
    string bulletTag = "BULLET";
    #endregion

    #region Components
    [SerializeField]
    private AudioSource _source;
    #endregion

    #region Resources Objects
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private AudioClip hitSound;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> ������Ʈ ���ε� </summary>
    private void Awake()
    {
        _source = GetComponent<AudioSource>();

        hitSound = Resources.Load<AudioClip>("Sound/bullet_hit_metal");
        hitEffect = Resources.Load<GameObject>("Effects/TinyExplosion_VFX");
    }

    /// <summary> �ݶ��̴� �浹 ���� �޼��� </summary>
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == bulletTag)
        {
            // if (brCtrl != null) brCtrl.hitCount++;

            Destroy(col.gameObject);

            // ù��° ���
            // Vector3 hitPos = col.contacts[0].point;
            //Quaternion hitRot = Quaternion.FromToRotation(Vector3.forward, hitPos.normalized);

            // �ι�° ���
            ContactPoint contactPoint = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contactPoint.normal);

            GameObject hitEff = Instantiate(hitEffect, contactPoint.point, rot);
            Destroy(hitEff, 1.5f);
            _source.PlayOneShot(hitSound);
        }
    }
    #endregion
}
