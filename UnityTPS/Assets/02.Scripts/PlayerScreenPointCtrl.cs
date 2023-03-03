using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScreenPointCtrl : MonoBehaviour
{
    private float h, v, r;
    private float moveSpeed = 3.5f;
    private float turnSpeed = 90.0f;
    private float jumpForce = 200.0f;

    [SerializeField] // ��Ʈ����Ʈ
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
        #region �÷��̾� �ִϸ��̼� ����
        // CrossFade : ���� ���� �ִϸ��̼ǰ� ���� �Ϸ��� ������ 0.3�� ���� ȥ���ؼ� �ε巴�� �ִϸ��̼��� �����.
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
        #region �÷��̾� �̵��� ȸ��
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
