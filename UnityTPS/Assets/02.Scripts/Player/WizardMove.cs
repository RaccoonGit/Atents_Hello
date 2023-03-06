using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardMove : MonoBehaviour
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
        #region 도착 지점과 자기 자신(위자드)의 거리를 재어서 하는 방법
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

        #region 네비 매시 에이전트를 이용하는 방법
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
        // 카메라에서 광선을 마우스 포인트 커서 쪽으로 발사
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.red);
        RaycastHit hit; // 광선에 맞은 면 맞은 위치 포인트를 알려주는 구조체
        if (Input.GetMouseButtonDown(0))
        {
            bool hasHit = Physics.Raycast(ray, out hit, Mathf.Infinity);

            if (hasHit)
            {
                // 추적 대상
                target = hit.point;

                agent.destination = target;
                Debug.Log($"좌표 {hit.point}");
            }
        }
    }
}