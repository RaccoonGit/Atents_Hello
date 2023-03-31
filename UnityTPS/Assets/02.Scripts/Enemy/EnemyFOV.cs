using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    //적캐릭터의 추적 사정 거리의 범위
    public float viewRange = 15.0f;
    [Range(0, 360)]
    //적캐릭터 시야각 
    public float viewAngle = 120f;

    private Transform enemyTr;
    private Transform playerTr;
    private int playerLayer;
    private int obstacleLayer;
    private int layerMask;
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        layerMask = 1 << playerLayer | 1 << obstacleLayer;
    }

    //주어진 각도에 의해 원주 위의 점의 좌표값을 계산 하는 함수
    public Vector3 CirclePoint(float angle)
    {
        //로컬 좌표계 기준으로 설정 하기 위해 적 캐릭터의 
        //Y 회전값을 더함 
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0,
            Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool isTracePlayer()
    {

        bool isTrace = false;
        //추적 반경  범위 안에  플레이어 캐릭터 추출 
        Collider[] cols = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);
        //배열의 갯수가 1일때 플레이어가 범위 안에 있다고 판단
        if (cols.Length == 1)
        {
            //적캐릭터와 주인공 사이의 방향 벡터를 계산 
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;
            //적캐릭터 시야 각에 들어 왔는 지를 판단
            if (Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }
        return isTrace;
    }

    public bool isViewPlayer()
    {

        bool isView = false;
        RaycastHit hit;

        Vector3 dir = (playerTr.position - enemyTr.position).normalized;
        //레이 캐스트를 투사 해서 장애물이 있는지 없는 지 여부 판단
        if (Physics.Raycast(enemyTr.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag("Player"));
        }
        return isView;
    }
}
