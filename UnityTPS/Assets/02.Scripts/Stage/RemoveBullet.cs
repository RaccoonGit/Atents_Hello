using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RemoveBullet : MonoBehaviour
{
    #region Tag String
    string bulletTag = "BULLET";
    string e_bulletTag = "E_BULLET";
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

    [SerializeField]
    FireCtrl fireCtrl;

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> 컴포넌트 바인딩 </summary>
    private void Awake()
    {
        _source = GetComponent<AudioSource>();

        hitSound = Resources.Load<AudioClip>("Sound/bullet_hit_metal");
        hitEffect = Resources.Load<GameObject>("Effects/TinyExplosion_VFX");
        StartCoroutine(GetCtrl());
    }
    
    IEnumerator GetCtrl()
    {
        while(!SceneManager.GetSceneByName("Level").isLoaded)
        {
            yield return null;
        }
        fireCtrl = GameObject.FindWithTag("Player").GetComponent<FireCtrl>();
    }

    void OnDamage(object[] _params)
    {
        if (fireCtrl.isReload) return;
        Vector3 hitPos = (Vector3)_params[0];
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitPos.normalized);
        GameObject hitEff = Instantiate(hitEffect, (Vector3)_params[0], rot);
        Destroy(hitEff, 1.5f);
        _source.PlayOneShot(hitSound, 0.5f);
    }
    /// <summary> 콜라이더 충돌 감지 메서드 </summary>
    //private void OnCollisionEnter(Collision col)
    //{
    //    if(col.collider.tag == bulletTag || col.collider.tag == e_bulletTag)
    //    {
    //        // if (brCtrl != null) brCtrl.hitCount++;

    //        // Destroy(col.gameObject);

    //        // 첫번째 방법
    //        // Vector3 hitPos = col.contacts[0].point;
    //        //Quaternion hitRot = Quaternion.FromToRotation(Vector3.forward, hitPos.normalized);
    //        // 두번째 방법
    //        ContactPoint contactPoint = col.contacts[0];
    //        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contactPoint.normal);

    //        GameObject hitEff = Instantiate(hitEffect, contactPoint.point, rot);
    //        Destroy(hitEff, 1.5f);
    //        _source.PlayOneShot(hitSound);
    //    }
    //}
    #endregion
}
