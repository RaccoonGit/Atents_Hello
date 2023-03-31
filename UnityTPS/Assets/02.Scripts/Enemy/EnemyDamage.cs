using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1. HP
// 2. 충돌 감지 함수
// 3. 충돌 혈흔 효과
// 4. HP 관련 UI
public class EnemyDamage : MonoBehaviour
{
    #region Tag String
    private readonly string bulletTag = "BULLET";
    #endregion

    #region Private Field
    public float hp = 100.0f;
    private readonly float hpInit = 100.0f;
    private Canvas _ui;
    #endregion

    #region Resources Objects
    [SerializeField]
    private GameObject bloodEffect;
    [Header("New Type UI")]
    [SerializeField]
    private GameObject hpBarPrefab;
    [SerializeField]
    private Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    [SerializeField]
    private Canvas uiCanvas;
    [SerializeField] 
    private Image hpBarImage;
    #endregion

    void Awake()
    {
        uiCanvas = GameObject.Find("Canvas-EnemyUI").GetComponent<Canvas>();
        bloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");
        // hpBar = transform.GetChild(4).GetComponentInChildren<Image>();
        hpBarPrefab = Resources.Load<GameObject>("Image-EnemyHpBar");
        SetHP();
    }

    private void SetHP()
    {
        GameObject hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        EnemyHpBar _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = transform;
        _hpBar.offset = hpBarOffset;
    }

    //private void OnCollisionEnter(Collision col)
    //{
    //    if(col.gameObject.tag == bulletTag)
    //    {
    //        // Destroy(col.gameObject);
    //        ShowBloodEffect(col);
    //        hp -= col.transform.GetComponent<BulletCtrl>().damage;
    //        if (hp <= 0) 
    //        {
    //            if (GetComponent<EnemyAI>() != null)
    //            {
    //                GetComponent<EnemyAI>().Die();
    //            }
    //            else if (GetComponent<S_EnemyAI>() != null)
    //            {
    //                GetComponent<S_EnemyAI>().Die();
    //            }
    //        }
    //    }
    //}

    void OnDamage(object[] _params)
    {
        ShowBloodEffect((Vector3)_params[0]);

        hp -= (float)_params[1];
        hpBarImage.fillAmount = hp / hpInit;

        if (hp <= 0)
        {
            hp = 0.0f;
            hpBarImage.fillAmount = hp / 100;
            if (hp <= 35.0f)
            {
                hpBarImage.color = Color.red;
            }
            else if (hp <= 70.0f)
            {
                hpBarImage.color = Color.yellow;
            }
            else
            {
                hpBarImage.color = Color.green;
            }

            if (GetComponent<EnemyAI>() != null)
            {
                GetComponent<EnemyAI>().Die();
            }
            else if (GetComponent<S_EnemyAI>() != null)
            {
                GetComponent<S_EnemyAI>().Die();
            }
            // 적 캐릭터가 사망한 이후에 생명 게이지를 투명 처리
            HPBarClear();
        }
    }

    private void HPBarClear()
    {
        hpBarImage.GetComponents<Image>()[0].color = Color.clear;
    }
    //private void ShowBloodEffect(Collision col)
    //{
    //    // ContactPoint contactPoint = col.contacts[0];
    //    Vector3 pos = col.contacts[0].point;
    //    Vector3 _normal = col.contacts[0].normal;
    //    Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
    //    GameObject blood = Instantiate(bloodEffect, pos, rot);
    //    Destroy(blood, 3.0f);
    //}

    private void ShowBloodEffect(Vector3 param)
    {
        Vector3 pos = param;
        Vector3 _normal = param.normalized;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 3.0f);
    }
}
