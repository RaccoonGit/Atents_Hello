using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    public Color lineColor = Color.black;
    public List<Transform> nodes;

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        // 효율적
        var Points = this;
        if(Points != null)
        {
            Points.GetComponentsInChildren<Transform>(nodes);
            nodes.RemoveAt(0);
        }

        //// 자기 자신부터 포함해서 자식 오브젝트의 트랜스폼을 Points라는 배열에 대입한다.
        //Transform[] Points = GetComponentsInChildren<Transform>();
        //nodes = new List<Transform>();

        //for(int i = 0; i < Points.Length; i++)
        //{
        //    if(Points[i] != this.transform)
        //    {
        //        nodes.Add(Points[i]);
        //    }
        //}

        for (int i = 0; i < nodes.Count; i++)
        {
            // 현재 노드
            Vector3 curNode = nodes[i].position;
            Vector3 PrevNode = Vector3.zero;

            if(i > 0)
            {
                PrevNode = nodes[i - 1].position;
            }
            else if(i == 0 && nodes.Count > 1)
            {
                PrevNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(PrevNode, curNode);

            Gizmos.DrawSphere(curNode, 0.3f);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
