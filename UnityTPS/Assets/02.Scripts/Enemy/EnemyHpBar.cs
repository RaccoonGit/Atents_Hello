using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera; // UI Canvas�� ������ �ϴ� ī�޶�
    private Canvas _canvas;
    private RectTransform rectParent;
    private RectTransform rectHP;
    public Vector3 offset = Vector3.zero;
    public Transform targetTr;

    void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
        uiCamera = _canvas.worldCamera.GetComponent<Camera>();
        rectParent = _canvas.GetComponent<RectTransform>();
        rectHP = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // �� ĳ������ ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
        // ��ũ�� ��ǥ�� ��ũ���� �������� ���� �Ʒ��� (0, 0)
        // ������ ���� (��ũ�� �ȼ� ��, ��ũ�� �ȼ� ����)�� ��ǥ���.
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        // ī�޶��� ���� ����(180�� ȸ��)�� �� ��ǥ�� ����

        // ���� ��ǥ���� ��ũ�� ��ǥ�� �Ѿ�� �� ������� �ʴ� z���� ī�޶�...
        // ��ũ�� ��ǥ2D��ǥ���̱� ������ ī�޶�(���ΰ�)�� 180�� ȸ���� �� ĳ���Ϳ�
        // ������ �ִ��� ȭ�鿡 ǥ�õȴ�. ���״� �ƴ����� ��ġ�ʴ� ����̴�.
        // ī�޶� 180�� ȸ�� �ߴ��� Z���� �������� �Ǹ� 180 �̻� ȸ���� ������ �Ǵ��� �� �ִ�.
        // ���� ScreenPos�� Z���� ������ �Ǹ� -1�� ���Ѵ�.
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        // ��ũ�� ��ǥ�� RectTransform ��ǥ������ ���� �޴� ����.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectParent,
            screenPos,
            uiCamera,
            out localPos)
            ;

        // ���� ������ �̹����� ��ġ ����
        rectHP.localPosition = localPos;
    }
}
