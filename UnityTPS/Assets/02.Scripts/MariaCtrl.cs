using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MariaCtrl : MonoBehaviour
{
    [SerializeField] [Tooltip("타겟")] private Vector3 target;
    [SerializeField] [Tooltip("네비게이션")] private NavMeshAgent agent;
    [SerializeField] [Tooltip("애니메이터")] private Animator anim;
    [SerializeField] [Tooltip("공격 상태")] private bool isAttack = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        MouseClickMove();
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetTrigger("AttackDash");
            agent.velocity = Vector3.zero;
        }

        else if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("AttackOne");
        }
    }

    private void MouseClickMove()
    {
        if (agent.remainingDistance <= 0.35f) agent.velocity = Vector3.zero;
        anim.SetFloat("MoveVelocity", agent.remainingDistance);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            bool hasHit = Physics.Raycast(ray, out hit, Mathf.Infinity);

            if (hasHit)
            {
                target = hit.point;
                agent.destination = target;
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.red);
    }
}
