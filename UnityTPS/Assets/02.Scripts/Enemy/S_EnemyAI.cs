using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyAI : MonoBehaviour
{
    public enum STATE { PATROL, TRACE, ATTACK, DIE }

    #region Animator Hashs
    // �ؽ� ���̺� �ִ� isMove�� ã�Ƽ� int ������ ��ȯ
    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashVelocity = Animator.StringToHash("MoveVelocity");
    private readonly int hashDie = Animator.StringToHash("isDie");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
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
    /// <summary> ������Ʈ �Ҵ� </summary>
    void Awake()
    {
        // S_MoveAgent Ŭ���� �Ҵ�
        moveAgent = GetComponent<S_MoveAgent>();
        // S_EnemyFire Ŭ���� �Ҵ�
        enemyFire = GetComponent<S_EnemyFire>();
        // Animator ������Ʈ �Ҵ�
        _animator = GetComponent<Animator>();


        // This.Transform
        Tr = GetComponent<Transform>();
        // �� ������Ʈ�� Transform
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        // WS ƽ ��ġ �ʱ�ȭ
        ws = new WaitForSeconds(0.25f);

        _animator.SetFloat(hashOffset, Random.Range(0.0f, 1.0f));
        _animator.SetFloat(hashWalkSpeed, Random.Range(1.0f, 1.5f));
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(EnemyAction());
        // �̺�Ʈ ����
        Damage.OnPlayerDie += this.OnPlayerDie;
        BarrelCtrl.OnEnemiesDie += this.Die;
    }
    private void OnDisable()
    {
        // �̺�Ʈ ���� ����
        Damage.OnPlayerDie -= this.OnPlayerDie;
        BarrelCtrl.OnEnemiesDie -= this.Die;
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

    public void Die()
    {
        if (isDie) return;
        moveAgent.Stop();
        isDie = true;
        enemyFire.isFire = false;
        moveAgent.patrolling = false;
        _animator.SetInteger(hashDieIdx, Random.Range(1, 3));
        _animator.SetTrigger(hashDie);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        StopAllCoroutines();
        this.gameObject.tag = "Untagged";
        GameManager.inst.IncKillCount();

        StartCoroutine(PushPool());

        foreach(GameObject hpObj in GameObject.FindGameObjectsWithTag("ENEMYHPBAR"))
        {
            Destroy(hpObj);
        }
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        StopAllCoroutines();
        _animator.SetTrigger(hashPlayerDie);
    }

    private IEnumerator PushPool()
    {
        yield return new WaitForSeconds(3.0f);
        isDie = false;
        GetComponent<EnemyDamage>().hp = 100.0f;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        curState = STATE.PATROL;
        gameObject.SetActive(false);
    }

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

    /// <summary> ������Ƽ�� ���� ���� �� Speed ������ Animator �Ķ���� ���� </summary>
    private IEnumerator SetAnimVelocity()
    {
        while (!isDie)
        {
            // ������Ƽ�� ���� ���� �� Speed ������ Animator �Ķ���� ����
            _animator.SetFloat(hashVelocity, moveAgent.speed);
            yield return null;
        }
    }
    #endregion
}
