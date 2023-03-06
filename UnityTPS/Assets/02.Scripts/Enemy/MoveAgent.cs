using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// �׺�޽� ������Ʈ ������Ʈ�� ������ ������ �߻���Ų��.
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    #region ReadOnly
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
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
    public List<Transform> wayPoints;
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
        var group = GameObject.Find("PatrolPoints");
        if (group != null) // ��ȿ�� �˻�
        {
            // ���� ����Ʈ�� Transform �� �޾ƿ���
            group.GetComponentsInChildren<Transform>(wayPoints);
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
        Debug.Log(agent.velocity.sqrMagnitude);
        // �������� �Ÿ��� 0.5���� ������, �̵�����¡ �˱� ����
        if(agent.remainingDistance <= 0.5f && agent.velocity.sqrMagnitude >= 0.2f)
        {
            nextIdx = ++nextIdx % wayPoints.Count;
            MoveWayPoint();
        }
    }
    #endregion

    /// <summary> ��ü�� ���� ����Ʈ�� �̵��ϴ� �޼ҵ� </summary>
    void MoveWayPoint()
    {
        // �ִ� ��ΰ� �˻����� ������ ����������.
        if (agent.isPathStale) return;
        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
    }

    /// <summary> ��ü�� ���� ��Ű�� �޼��� </summary>
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    /// <summary> Ÿ���� ��ġ�� ��ü�� �̵� ��Ű�� �޼��� </summary>
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
}