using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera; // UI Canvas를 렌더링 하는 카메라
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
        // 적 캐릭터의 월드 좌표를 스크린 좌표로 변환
        // 스크린 좌표는 스크린을 기준으로 왼족 아래가 (0, 0)
        // 오른쪽 위가 (스크린 픽셀 폭, 스크린 픽셀 높이)인 좌표계다.
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        // 카메라의 뒤쪽 영역(180도 회전)일 때 좌표값 보정

        // 월드 좌표에서 스크린 좌표로 넘어올 때 사용하지 않는 z값은 카메라에...
        // 스크린 좌표2D좌표계이기 때문에 카메라(주인공)가 180도 회전해 적 캐릭터와
        // 등지고 있더라도 화면에 표시된다. 버그는 아니지만 원치않는 결과이다.
        // 카메라가 180도 회전 했는지 Z값이 음수값이 되면 180 이상 회전한 것으로 판단할 수 있다.
        // 따라서 ScreenPos의 Z값이 음수가 되면 -1을 곱한다.
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        // 스크린 좌표를 RectTransform 좌표값으로 전달 받는 변수.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectParent,
            screenPos,
            uiCamera,
            out localPos)
            ;

        // 생명 게이지 이미지의 위치 변경
        rectHP.localPosition = localPos;
    }
}
