using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyAI : MonoBehaviour
{
    public enum STATE { PATROL, TRACE, ATTACK, DIE }

    #region Animator Hashs
    // 해시 테이블에 있는 isMove를 찾아서 int 값으로 변환
    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashVelocity = Animator.StringToHash("MoveVelocity");
    #endregion

    #region Comonenets
    private Animator _animator;
    #endregion

    #region Classes
    private S_EnemyFire enemyFire;
    private S_MoveAgent moveAgent;
    #endregion

    #region State Machine
    public STATE curState = STATE.PATROL;
    #endregion

    #region Private Fields
    private Color _attcolor = Color.red;
    private Color _tracecolor = Color.blue;
    private new WaitForSeconds ws;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform Tr;
    #endregion

    #region Public Fields
    public float traceDist = 15.0f;
    public float attackDist = 10.0f;
    public bool isDie = false;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> 컴포넌트 할당 </summary>
    void Awake()
    {
        // S_MoveAgent 클래스 할당
        moveAgent = GetComponent<S_MoveAgent>();
        // S_EnemyFire 클래스 할당
        enemyFire = GetComponent<S_EnemyFire>();
        // Animator 컴포넌트 할당
        _animator = GetComponent<Animator>();


        // This.Transform
        Tr = GetComponent<Transform>();
        // 적 오브젝트의 Transform
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // WS 틱 수치 초기화
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
        // 프로퍼티에 의해 조절 된 Speed 값으로 Animator 파라미터 조절
        _animator.SetFloat(hashVelocity, moveAgent.speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _attcolor;
        Gizmos.DrawWireSphere(Tr.position, attackDist);

        Gizmos.color = _tracecolor;
        Gizmos.DrawWireSphere(Tr.position, traceDist);

    }
    #endregion

    /***********************************************************************
    *                               Coroutines
    ***********************************************************************/
    #region Coroutines
    /// <summary> WS 틱마다 CurState 상태 체크 </summary>
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

    /// <summary> 실질적 액션을 처리하는 비 동기 코루틴 </summary>
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
                    break;
                case STATE.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = player.position;
                    _animator.SetBool(hashMove, true);
                    break;
                case STATE.ATTACK:
                    moveAgent.Stop();
                    _animator.SetBool(hashMove, false);
                    if (!enemyFire.isFire)
                        enemyFire.isFire = true;
                    break;

                case STATE.DIE:
                    break;
            }
        }
    }

    /// <summary> 프로퍼티에 의해 조절 된 Speed 값으로 Animator 파라미터 조절 </summary>
    private IEnumerator SetAnimVelocity()
    {
        while (!isDie)
        {
            // 프로퍼티에 의해 조절 된 Speed 값으로 Animator 파라미터 조절
            _animator.SetFloat(hashVelocity, moveAgent.speed);
            yield return null;
        }
    }
    #endregion
}
