using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserBeam : MonoBehaviour
{
    private Transform Tr;
    private LineRenderer line;
    private RaycastHit hit;
    private const float rayPos = 0.065f;

    private FireCtrl fireCtrl;

    void Start()
    {
        Tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();
        fireCtrl = GetComponentInParent<FireCtrl>();
        line.useWorldSpace = false;
        line.enabled = false;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Ray ray = new Ray(Tr.position + Vector3.up * rayPos, Tr.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red);
        if(Input.GetMouseButtonDown(0) && !fireCtrl.isReload)
        {
            // ���忡�� ���÷� ��ǥ ��ȯ
            line.SetPosition(0, Tr.InverseTransformPoint(ray.origin));

            if(Physics.Raycast(ray, out hit))
            {
                // ���� ������ �ι�° �� ��ġ ����
                line.SetPosition(1, Tr.InverseTransformPoint(hit.point));
            }
            else
            {
                line.SetPosition(1, Tr.InverseTransformPoint(ray.GetPoint(100.0f)));
            }
            StartCoroutine(ShowLaserBeam());
        }
    }

    private IEnumerator ShowLaserBeam()
    {
        line.enabled = true;
        yield return new WaitForSeconds(Random.Range(0.015f, 0.25f));
        line.enabled = false;
    }
}
