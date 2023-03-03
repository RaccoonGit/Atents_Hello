using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatCtrl : MonoBehaviour
{
    private float h, v, r;
    private float moveSpeed = 3.5f;
    private float turnSpeed = 90.0f;
    private float jumpForce = 90.0f;
    private bool isJump = false;

    [SerializeField] // 어트리뷰트
    private Animator anim;
    [SerializeField]
    private Transform tr;
    [SerializeField]
    private Rigidbody rbody;
    [SerializeField]
    private CapsuleCollider capCol;
    void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        rbody = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 8.0f;
            anim.SetBool("IsRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 3.5f;
            anim.SetBool("IsRun", false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && h ==0 && v==0)
        {
            rbody.velocity = Vector3.up * 1.5f;
            anim.SetTrigger("Jump");
        }
        if (Input.GetKeyDown(KeyCode.Space) && h == 0 && v >= 0.1f)
        {
            rbody.velocity = Vector3.up * 1.5f;
            anim.SetTrigger("RunJump");
        }
    }

    private void FixedUpdate()
    {
        #region 플레이어 이동과 회전
        MoveAndRotate();
        #endregion
    }

    private void MoveAndRotate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        transform.Translate(Vector3.right.normalized * h * Time.deltaTime * moveSpeed);
        {
            anim.SetFloat("SpeedX", h, 0.01f, Time.deltaTime);
        }
        transform.Translate(Vector3.forward.normalized * v * Time.deltaTime * moveSpeed);
        {
            anim.SetFloat("SpeedY", v, 0.01f, Time.deltaTime);
        }
        tr.Rotate(Vector3.up * r * Time.deltaTime * turnSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isJump = false;
    }
}
