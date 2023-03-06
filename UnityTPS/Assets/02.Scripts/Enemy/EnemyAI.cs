using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum STATE { PATROL, TRACE, ATTACK, DIE }

    #region Animator Hashs
    // �ؽ� ���̺� �ִ� isMove�� ã�Ƽ� int ������ ��ȯ
    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashVelocity = Animator.StringToHash("MoveVelocity");
    #endregion

    #region Comonenets
    private Animator animator;
    #endregion

    #region Classes
    private MoveAgent moveAgent;
    #endregion

    #region State Machine
    public STATE curState = STATE.PATROL;
    #endregion

    #region Private Fields
    private new WaitForSeconds ws;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform Tr;
    #endregion

    #region Public Fields
    public float traceDist = 10.0f;
    public float attackDist = 5.0f;
    public bool isDie = false;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> ������Ʈ �Ҵ� </summary>
    void Awake()
    {
        // Move Agent Ŭ���� �Ҵ�
        moveAgent = GetComponent<MoveAgent>();
        // Animator ������Ʈ �Ҵ�
        animator = GetComponent<Animator>();

        // This.Transform
        Tr = GetComponent<Transform>();
        // �� ������Ʈ�� Transform
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // WS ƽ ��ġ �ʱ�ȭ
        ws = new WaitForSeconds(0.25f);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(EnemyAction());
    }

    private void Update()
    {
        // ������Ƽ�� ���� ���� �� Speed ������ Animator �Ķ���� ����
        animator.SetFloat(hashVelocity, moveAgent.speed);
    }
    #endregion

    /***********************************************************************
    *                               Coroutines
    ***********************************************************************/
    #region Coroutines
    /// <summary> WS ƽ���� CurState ���� üũ </summary>
    private IEnumerator CheckState()
    {
        while (!isDie)
        {
            float dist = Vector3.Distance(Tr.position, player.position);
            if (dist <= attackDist)
            {
                curState = STATE.ATTACK;
            }
            else if (dist <= traceDist)
            {
                curState = STATE.TRACE;
            }
            else
            {
                curState = STATE.PATROL;
            }
            yield return ws;
        }
    }

    /// <summary> ������ �׼��� ó���ϴ� �� ���� �ڷ�ƾ </summary>
    private IEnumerator EnemyAction()
    {
        while(!isDie)
        {
            yield return ws;
            switch (curState)
            {
                case STATE.PATROL:
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.TRACE:
                    moveAgent.traceTarget = player.position;
                    animator.SetBool(hashMove, true);
                    break;
                case STATE.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    break;

                case STATE.DIE:
                    break;
            }
        }
    }

    /// <summary> ������Ƽ�� ���� ���� �� Speed ������ Animator �Ķ���� ���� </summary>
    private IEnumerator SetAnimVelocity()
    {
        while(!isDie)
        {
            // ������Ƽ�� ���� ���� �� Speed ������ Animator �Ķ���� ����
            animator.SetFloat(hashVelocity, moveAgent.speed);
            yield return null;
        }
    }
    #endregion
}
