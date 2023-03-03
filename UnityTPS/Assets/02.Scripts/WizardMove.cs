using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardMove : MonoBehaviour
{
    [SerializeField] [Tooltip("Ÿ��")] private Vector3 target;
    [SerializeField] [Tooltip("�׺���̼�")] private NavMeshAgent agent;
    [SerializeField] [Tooltip("�ִϸ�����")] private Animator anim;
    [SerializeField] [Tooltip("���� ����")] private bool isAttack = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {        
        Attack();
        Distance();
        MouseClickMove();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetTrigger("SkillOne");
        }

        else if (Input.GetMouseButtonDown(1))
        {
            if (!isAttack)
            {
                anim.SetTrigger("AttackOne");
            }
        }
    }

    private void Distance()
    {
        #region ���� ������ �ڱ� �ڽ�(���ڵ�)�� �Ÿ��� �� �ϴ� ���
        //if (Vector3.Distance(transform.position, target) > 0.35f)
        //{
        //    anim.SetBool("isWalk", true);
        //}
        //else
        //{
        //    anim.SetBool("isWalk", false);
        //    agent.velocity = Vector3.zero;
        //}
        #endregion

        #region �׺� �Ž� ������Ʈ�� �̿��ϴ� ���
        if (agent.remainingDistance > 0.35f)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
            // agent.velocity = Vector3.zero;
        }
        #endregion
    }

    private void MouseClickMove()
    {
        // ī�޶󿡼� ������ ���콺 ����Ʈ Ŀ�� ������ �߻�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.red);
        RaycastHit hit; // ������ ���� �� ���� ��ġ ����Ʈ�� �˷��ִ� ����ü
        if (Input.GetMouseButtonDown(0))
        {
            bool hasHit = Physics.Raycast(ray, out hit, Mathf.Infinity);

            if (hasHit)
            {
                // ���� ���
                target = hit.point;

                agent.destination = target;
                Debug.Log($"��ǥ {hit.point}");
            }
        }
    }
}