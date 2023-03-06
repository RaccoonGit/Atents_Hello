using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region Components
    [SerializeField] // 어트리뷰트
    private Animation anim;
    [SerializeField]
    private Transform tr;
    #endregion

    #region Private Fields
    private float h, v, r;
    private float moveSpeed = 3.5f;
    private float turnSpeed = 90.0f;
    #endregion

    #region Public Fields
    public bool isRun = false;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    void Start()
    {
        anim = GetComponent<Animation>();
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        #region 플레이어 애니메이션 연동
        // CrossFade : 직전 동작 애니메이션과 지금 하려는 동작을 0.3초 동안 혼합해서 부드럽운 애니메이션을 만든다.

        if(h > 0.1f)
        {
            anim.CrossFade("RunR", 0.3f);
        }
        else if(h < -0.1f)
        {
            anim.CrossFade("RunL", 0.3f);
        }
        else if (v > 0.1f)
        {
            anim.CrossFade("RunF", 0.3f);
        }
        else if (v < -0.1f)
        {
            anim.CrossFade("RunB", 0.3f);
        }
        else
        {
            anim.CrossFade("Idle", 0.3f);
        }

        if (Input.GetKey(KeyCode.LeftShift) && v > 0.1f)
        {
            anim.CrossFade("SprintF", 0.3f);
            isRun = true;
            moveSpeed = 8.0f;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) || v <= 0.0f)
        {
            isRun = false;
            moveSpeed = 3.5f;
        }
        #endregion

    }

    private void FixedUpdate()
    {
        #region 플레이어 이동과 회전
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.right * h) + (Vector3.forward * v);
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        //transform.Translate(Vector3.right * h * Time.deltaTime * moveSpeed);
        //transform.Translate(Vector3.forward * v * Time.deltaTime * moveSpeed);
        tr.Rotate(Vector3.up * r * Time.deltaTime * turnSpeed);
        #endregion
    }
    #endregion
}
