using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharaState
{
    STAND, CROUCH, PRONE
}

public class VanguardCtrl : MonoBehaviour
{
    // ���� �ؾ� �� ��
    // 1. ���� ���¿��� �Ͼ� �� �� �ִϸ��̼��� ���� �� ���� �̵� ���� �ɱ�
    // 2. �ִϸ��̼� ���̾� ���� (���� �޸ӳ��̵� �ִϸ��̼����� ��ü�ϰų�, ������ �ִϸ��̼��� ���ؾ��Ѵ�.)
    // 3. �ִϸ����� ���� �б� �ܼ�ȭ, ��ũ��Ʈ ���� �б� �ܼ�ȭ

    #region this
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform tr;
    [SerializeField]
    private Rigidbody rbody;
    [SerializeField]
    private CapsuleCollider capCol;
    #endregion

    #region Private Properties
    [SerializeField]
    private CharaState state = CharaState.STAND;
    private float horizontal, vertical, row, col;
    [SerializeField]
    private float moveSpeed = 3.5f;
    private float turnSpeed = 90.0f;
    private float jumpForce = 90.0f;
    #endregion

    #region Condition Bool
    [SerializeField]
    private bool isFire = false;
    [SerializeField]
    private bool isCrouch = false;
    [SerializeField]
    private bool isProne = false;
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        rbody = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        // �ӵ� ����
        // 1. ĳ���Ͱ� �� �ִ� ���
        if(!anim.GetBool("isCrouch") && !anim.GetBool("isProne"))
        {
            state = CharaState.STAND;
            moveSpeed = 3.5f;
        }
        // 2. ĳ���Ͱ� �ɾ� �ִ� ���
        else if (anim.GetBool("isCrouch") && !anim.GetBool("isProne"))
        {
            state = CharaState.CROUCH;
            moveSpeed = 2.5f;
        }
        // 3. ĳ���Ͱ� ���� �ִ� ���
        else if (anim.GetBool("isCrouch") && anim.GetBool("isProne"))
        {
            state = CharaState.PRONE;
            moveSpeed = 1.0f;
        }

        // ��� �ִϸ��̼�
        if (Input.GetMouseButton(0))
        {
            isFire = true;
            anim.SetBool("isFire", isFire);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isFire = false;
            anim.SetBool("isFire", isFire);
        }

        // �ɱ� �ִϸ��̼�
        // 1. ĳ���Ͱ� �� �ִ� ���
        if (Input.GetKeyDown(KeyCode.C) && !isCrouch && !isProne)
        {
            isCrouch = true;
            anim.SetBool("isCrouch", isCrouch);
        }
        // 2. ĳ���Ͱ� ���� �ִ� ���
        else if (Input.GetKeyDown(KeyCode.C) && isProne && isProne)
        {
            isCrouch = true;
            isProne = false;
            anim.SetBool("isCrouch", isCrouch);
            anim.SetBool("isProne", isProne);
        }
        // 3. ĳ���Ͱ� �̹� �ɾ� �ִ� ���
        else if (Input.GetKeyDown(KeyCode.C) && isCrouch && !isProne)
        {
            isCrouch = false;
            anim.SetBool("isCrouch", isCrouch);
        }

        // ���� �ִϸ��̼�
        // 1. ĳ���Ͱ� �� �ִ� ���
        if (Input.GetKeyDown(KeyCode.X) && !isCrouch && !isProne)
        {
            isCrouch = true;
            isProne = true;
            anim.SetBool("ProneAction", true);
            anim.SetBool("isCrouch", isCrouch);
            anim.SetBool("isProne", isProne);
        }
        // 2. ĳ���Ͱ� �ɾ� �ִ� ���
        else if (Input.GetKeyDown(KeyCode.X) && isCrouch && !isProne)
        {
            isProne = true;
            anim.SetBool("ProneAction", true);
            anim.SetBool("isProne", isProne);
        }
        // 3. ĳ���Ͱ� �̹� �����ִ� ���
        else if (Input.GetKeyDown(KeyCode.X) && isCrouch && isProne)
        {
            isCrouch = false;
            isProne = false;
            anim.SetBool("ProneAction", true);
            anim.SetBool("isCrouch", isCrouch);
            anim.SetBool("isProne", isProne);
        }

        // ������ �ִϸ��̼�
        if (Input.GetKeyDown(KeyCode.R) && isProne)
        {
            anim.SetBool("ProneAction", true);
            anim.SetTrigger("Reload");
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Reload");
        }

        // ����ź �ִϸ��̼�
        if (Input.GetKeyDown(KeyCode.G) && isProne)
        {
            anim.SetBool("ProneAction", true);
            anim.SetTrigger("Grenade");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            anim.SetTrigger("Grenade");
        }
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case CharaState.STAND:
                MoveAndRotate();
                break;
            case CharaState.CROUCH:
                MoveAndRotate();
                break;
            case CharaState.PRONE:
                if(!isFire)
                    MoveAndRotate(false, anim. GetBool("ProneAction"));
                break;

        }
    }
    private void MoveAndRotate(bool proneCheck = true, bool proneAction = false)
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        row = Input.GetAxis("Mouse X");
        col = Input.GetAxis("Mouse Y");

        float moveDelta = moveSpeed * Time.deltaTime;
        float turnDelta = turnSpeed * Time.deltaTime;

        if (proneCheck && !proneAction)
            transform.Translate(Vector3.right.normalized * horizontal * moveDelta);
        {
            anim.SetFloat("X", horizontal, 0.01f, Time.deltaTime);
        }
        if (!proneAction) 
            transform.Translate(Vector3.forward.normalized * vertical * moveDelta);
        {
            anim.SetFloat("Z", vertical, 0.01f, Time.deltaTime);
        }
        tr.Rotate(Vector3.up * row * turnDelta);
    }

}
