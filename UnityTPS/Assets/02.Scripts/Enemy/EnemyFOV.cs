using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    //��ĳ������ ���� ���� �Ÿ��� ����
    public float viewRange = 15.0f;
    [Range(0, 360)]
    //��ĳ���� �þ߰� 
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

    //�־��� ������ ���� ���� ���� ���� ��ǥ���� ��� �ϴ� �Լ�
    public Vector3 CirclePoint(float angle)
    {
        //���� ��ǥ�� �������� ���� �ϱ� ���� �� ĳ������ 
        //Y ȸ������ ���� 
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0,
            Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool isTracePlayer()
    {

        bool isTrace = false;
        //���� �ݰ�  ���� �ȿ�  �÷��̾� ĳ���� ���� 
        Collider[] cols = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);
        //�迭�� ������ 1�϶� �÷��̾ ���� �ȿ� �ִٰ� �Ǵ�
        if (cols.Length == 1)
        {
            //��ĳ���Ϳ� ���ΰ� ������ ���� ���͸� ��� 
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;
            //��ĳ���� �þ� ���� ��� �Դ� ���� �Ǵ�
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
        //���� ĳ��Ʈ�� ���� �ؼ� ��ֹ��� �ִ��� ���� �� ���� �Ǵ�
        if (Physics.Raycast(enemyTr.position, dir, out hit, viewRange, layerMask))
        {
            isView = (hit.collider.CompareTag("Player"));
        }
        return isView;
    }
}
