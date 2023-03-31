using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform Tr;
    float rotDamping = 10.0f;
    float moveDamping = 15.0f;

    [SerializeField]
    private Transform target; // Ÿ��

    [SerializeField]
    private float height = 5.0f; // ī�޶� ����

    [SerializeField]
    private float distance = 7.0f; // ī�޶� �Ÿ�
    void Start()
    {
        Tr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);

        Tr.position = Vector3.Slerp(Tr.position, camPos, Time.deltaTime * moveDamping);

        Tr.rotation = Quaternion.Slerp(Tr.rotation, target.rotation, Time.deltaTime * rotDamping);

        Tr.LookAt(target.position + (target.up * 1f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position + (target.up * 1.0f), 0.1f);
        Gizmos.DrawLine(target.position + (target.up * 1.0f), transform.position);
    }
}
