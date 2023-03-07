using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_MoveAgent : MonoBehaviour
{
    #region ReadOnly
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    #endregion

    #region Components
    private NavMeshAgent _agent;
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
                _agent.speed = patrolSpeed;
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
            _agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    /// <summary> ���� �ӵ� ������Ƽ </summary>
    public float speed
    {
        // �׺� �Ž� ������Ʈ�� �ӵ��� ��ȯ
        get { return _agent.velocity.magnitude; }

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
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        // �׺� �Ž� ������Ʈ�� �극��ũ�� ������� �ʰڴ�.
        _agent.autoBraking = false;
        // �׺� �Ž� ������Ʈ�� ȸ���� ������� �ʰڴ�.
        _agent.updateRotation = false;

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
        if (_agent.isStopped == false)
        {
            // ������
            Quaternion rot = Quaternion.LookRotation(_agent.desiredVelocity);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
        }

        // �������϶��� �۵�
        if (!_patrolling) return;
        // �������� �Ÿ��� 0.5���� ������, �̵�����¡ �˱� ����
        if (_agent.remainingDistance <= 0.5f && _agent.velocity.sqrMagnitude >= 0.2f * 0.2f)
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
        if (_agent.isPathStale) return;
        _agent.destination = wayPoints[nextIdx].position;
        _agent.isStopped = false;
    }

    /// <summary> Ÿ���� ��ġ�� ��ü�� �̵� ��Ű�� �޼��� </summary>
    private void TraceTarget(Vector3 pos)
    {
        if (_agent.isPathStale) return;
        _agent.destination = pos;
        _agent.isStopped = false;
    }
    #endregion

    /***********************************************************************
    *                            Public Methods
    ***********************************************************************/
    #region Public Methods
    /// <summary> ��ü�� ���� ��Ű�� �޼��� </summary>
    public void Stop()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _patrolling = false;
    }
    #endregion
}
