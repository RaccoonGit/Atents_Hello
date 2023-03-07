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

    // 프로퍼티 : 멤버변수 보호를 위해 원본을 훼손하기 싫을 때
    #region Properties
    private bool _patrolling = false;
    /// <summary> 순찰 상태 체크 프로퍼티 </summary>
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
    /// <summary> 추적 대상 프로퍼티 </summary>
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

    /// <summary> 현재 속도 프로퍼티 </summary>
    public float speed
    {
        // 네비 매시 에이전트의 속도를 반환
        get { return _agent.velocity.magnitude; }

    }
    #endregion

    #region Private Fields
    // 회전할 때의 속도 보정
    private float damping = 1.0f;
    #endregion

    #region Public Fields
    public Transform waypointCore;
    // 패트롤 위치 리스트
    public List<Transform> wayPoints;
    // 다음 패트롤 위치 인덱스
    public int nextIdx;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> 컴포넌트 할당 </summary>
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        // 네비 매시 에이전트의 브레이크를 사용하지 않겠다.
        _agent.autoBraking = false;
        // 네비 매시 에이전트의 회전을 사용하지 않겠다.
        _agent.updateRotation = false;

        // 순찰 포인트 탐색
        if (waypointCore != null) // 유효성 검사
        {
            // 순찰 포인트의 Transform 값 받아오기
            waypointCore.GetComponentsInChildren<Transform>(wayPoints);
            // 순찰 포인트의 부모 객체 삭제
            wayPoints.RemoveAt(0);
        }
        // MoveWayPoint();
    }

    void Update()
    {
        // 적 캐릭터가 이동중일때만
        if (_agent.isStopped == false)
        {
            // 보간법
            Quaternion rot = Quaternion.LookRotation(_agent.desiredVelocity);

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
        }

        // 순찰중일때만 작동
        if (!_patrolling) return;
        // 목적지의 거리가 0.5보다 작으면, 이동중인징 알기 위해
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
    /// <summary> 객체를 순찰 포인트로 이동하는 메소드 </summary>
    private void MoveWayPoint()
    {
        // 최단 경로가 검색되지 않으면 빠져나간다.
        if (_agent.isPathStale) return;
        _agent.destination = wayPoints[nextIdx].position;
        _agent.isStopped = false;
    }

    /// <summary> 타겟의 위치로 객체를 이동 시키는 메서드 </summary>
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
    /// <summary> 객체를 정지 시키는 메서드 </summary>
    public void Stop()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _patrolling = false;
    }
    #endregion
}
