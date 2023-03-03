using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScreenPointCtrl : MonoBehaviour
{
    private float h, v, r;
    private float moveSpeed = 3.5f;
    private float turnSpeed = 90.0f;
    private float jumpForce = 200.0f;

    [SerializeField] // 어트리뷰트
    private Animation anim;
    [SerializeField]
    private Transform tr;
    [SerializeField]
    private Rigidbody rbody;

    void Start()
    {
        anim = GetComponent<Animation>();
        tr = GetComponent<Transform>();
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        #region 플레이어 애니메이션 연동
        // CrossFade : 직전 동작 애니메이션과 지금 하려는 동작을 0.3초 동안 혼합해서 부드럽운 애니메이션을 만든다.
        if (v > 0.1f)
        {
            anim.CrossFade("M_walk", 0.3f);
        }
        else if (v < -0.1f)
        {
            anim.CrossFade("M_walking_back", 0.3f);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            anim.Play("M_jump_runing");
        }
        else
        {
            anim.CrossFade("M_Standing-Free_A", 0.3f);
        }

        if (Input.GetKey(KeyCode.LeftShift) && v > 0.1f)
        {
            anim.CrossFade("M_Run", 0.3f);
            moveSpeed = 8.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || v <= 0.0f)
        {
            moveSpeed = 3.5f;
        }
        #endregion

    }

    private void FixedUpdate()
    {
        #region 플레이어 이동과 회전
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v);
        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        tr.Rotate(Vector3.up * h * Time.deltaTime * turnSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {

        }
        #endregion
    }
}
