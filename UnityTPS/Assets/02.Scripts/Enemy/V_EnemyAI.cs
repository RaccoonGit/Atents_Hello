using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_EnemyAI : MonoBehaviour
{
    public enum STATE { PATROL, TRACE, ATTACK, MELEETRACE, MELEE, DIE }

    #region Animator Hashs
    // �ؽ� ���̺� �ִ� isMove�� ã�Ƽ� int ������ ��ȯ
    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashVelocity = Animator.StringToHash("MoveVelocity");
    #endregion

    #region Comonenets
    private Animator _animator;
    #endregion

    #region Classes
    private V_EnemyFire enemyFire;
    private V_MoveAgent moveAgent;
    #endregion

    #region State Machine
    public STATE curState = STATE.PATROL;
    #endregion

    #region Private Fields
    private Color _attcolor = Color.red;
    private Color _tracecolor = Color.blue;
    private new WaitForSeconds ws;
    [SerializeField]
    public Transform player;
    [SerializeField]
    private Transform Tr;
    #endregion

    #region Public Fields
    public float traceDist = 25.0f;
    public float attackDist = 15.0f;
    public float meleeChaseDist = 4.0f;
    public float meleeDist = 1.5f;
    public bool isDie = false;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> ������Ʈ �Ҵ� </summary>
    void Awake()
    {
        // S_MoveAgent Ŭ���� �Ҵ�
        moveAgent = GetComponent<V_MoveAgent>();
        // S_EnemyFire Ŭ���� �Ҵ�
        enemyFire = GetComponent<V_EnemyFire>();
        // Animator ������Ʈ �Ҵ�
        _animator = GetComponent<Animator>();


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
        // StartCoroutine(SetAnimVelocity());
    }

    private void Update()
    {
        // ������Ƽ�� ���� ���� �� Speed ������ Animator �Ķ���� ����
        _animator.SetFloat(hashVelocity, moveAgent.speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _attcolor;
        Gizmos.DrawWireSphere(transform.position, attackDist);

        Gizmos.color = _tracecolor;
        Gizmos.DrawWireSphere(transform.position, traceDist);

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
            if (dist <= meleeDist)
            {
                curState = STATE.MELEE;
            }
            else if (dist <= meleeChaseDist)
            {
                curState = STATE.MELEETRACE;
            }
            else if (dist <= attackDist)
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
        while (!isDie)
        {
            yield return ws;
            switch (curState)
            {
                case STATE.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    _animator.SetBool(hashMove, true);
                    if (enemyFire.isFire)
                        enemyFire.isFire = false;
                    if (enemyFire.isMelee)
                        enemyFire.isMelee = false;
                    break;
                case STATE.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = player.position;
                    _animator.SetBool(hashMove, true);
                    if (enemyFire.isFire)
                        enemyFire.isFire = false;
                    if (enemyFire.isMelee)
                        enemyFire.isMelee = false;
                    break;
                case STATE.ATTACK:
                    moveAgent.Stop();
                    _animator.SetBool(hashMove, false);
                    if (!enemyFire.isFire)
                        enemyFire.isFire = true;
                    if (enemyFire.isMelee)
                        enemyFire.isMelee = false;
                    break;
                case STATE.MELEETRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = player.position;
                    _animator.SetBool(hashMove, true);
                    break;
                case STATE.MELEE:
                    moveAgent.Stop();
                    _animator.SetBool(hashMove, false);
                    if (enemyFire.isFire)
                        enemyFire.isFire = false;
                    if (!enemyFire.isMelee)
                        enemyFire.isMelee = true;
                    break;
                case STATE.DIE:
                    break;
            }
        }
    }
    #endregion
}
