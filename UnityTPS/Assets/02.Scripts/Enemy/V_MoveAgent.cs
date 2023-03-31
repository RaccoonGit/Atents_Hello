using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class V_MoveAgent : MonoBehaviour
{
    #region ReadOnly
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private readonly float meleeSpeed = 5.0f;
    #endregion

    #region Components
    private NavMeshAgent agent;
    #endregion

    // ������Ƽ : ������� ��ȣ�� ���� ������ �Ѽ��ϱ� ���� ��
    #region Properties
    private bool _patrolling = false;
    /// <summary> ���� ���� üũ ������Ƽ </summary>
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 3.0f;
                MoveWayPoint();
            }
        }
    }

    private bool _meleeChase;
    public bool meleeChase
    {
        get { return _meleeChase; }
        set
        {
            _meleeChase = value;
            if(_meleeChase)
            {
                agent.speed = meleeSpeed;
                damping = 5.0f;
            }
        }
    }

    private Vector3 _traceTarget;
    /// <summary> ���� ��� ������Ƽ </summary>
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    /// <summary> ���� �ӵ� ������Ƽ </summary>
    public float speed
    {
        // �׺� �Ž� ������Ʈ�� �ӵ��� ��ȯ
        get { return agent.velocity.magnitude; }

    }
    #endregion

    #region Private Fields
    // ȸ���� ���� �ӵ� ����
    private float damping = 1.0f;
    #endregion

    #region Public Fields
    public Transform waypointCore;
    // ��Ʈ�� ��ġ ����Ʈ
    public List<Transform> wayPoints;
    // ���� ��Ʈ�� ��ġ �ε���
    public int nextIdx;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> ������Ʈ �Ҵ� </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        // �׺� �Ž� ������Ʈ�� �극��ũ�� ������� �ʰڴ�.
        agent.autoBraking = false;
        // �׺� �Ž� ������Ʈ�� ȸ���� ������� �ʰڴ�.
        agent.updateRotation = false;

        // ���� ����Ʈ Ž��
        if (waypointCore != null) // ��ȿ�� �˻�
        {
            // ���� ����Ʈ�� Transform �� �޾ƿ���
            waypointCore.GetComponentsInChildren<Transform>(wayPoints);
            // ���� ����Ʈ�� �θ� ��ü ����
            wayPoints.RemoveAt(0);
        }
        // MoveWayPoint();
    }

    void Update()
    {
        // �� ĳ���Ͱ� �̵����϶���
        if (agent.isStopped == false)
        {
            // ������
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
        }

        // �������϶��� �۵�
        if (!_patrolling) return;
        // �������� �Ÿ��� 0.5���� ������, �̵�����¡ �˱� ����
        if (agent.remainingDistance <= 0.5f && agent.velocity.sqrMagnitude >= 0.2f * 0.2f)
        {
            nextIdx = ++nextIdx % wayPoints.Count;
            MoveWayPoint();
        }
    }
    #endregion

    /***********************************************************************
    *                            Private Methods
    ***********************************************************************/
    #region Private Methods
    /// <summary> ��ü�� ���� ����Ʈ�� �̵��ϴ� �޼ҵ� </summary>
    private void MoveWayPoint()
    {
        // �ִ� ��ΰ� �˻����� ������ ����������.
        if (agent.isPathStale) return;
        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
    }

    /// <summary> Ÿ���� ��ġ�� ��ü�� �̵� ��Ű�� �޼��� </summary>
    private void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
    #endregion

    /***********************************************************************
    *                            Public Methods
    ***********************************************************************/
    #region Public Methods
    /// <summary> ��ü�� ���� ��Ű�� �޼��� </summary>
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
    #endregion
}
