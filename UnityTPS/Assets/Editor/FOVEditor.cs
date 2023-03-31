using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Editor Ŭ������ ����Ƽ IDE�� �����ϴ� API �����̸�
//��Ÿ�� �ÿ� �ٸ� ��ũ��Ʈ���� ���� �� �� ����.
//�� ������ �������� �� ��� �� �� �ִ� Ŭ����
//�ݵ�� Editor��� ���� �ȿ��� ��� �ؾ� �� 
[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        //EnemyFOV Ŭ������ ���� 
        EnemyFOV fov = (EnemyFOV)target;
        // ���� ���� �������� ��ǥ�� ��� (�־��� ������ 1/2)
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        //���� ������ ������� ���� 
        Handles.color = Color.white;

        //�ܰ����� ǥ���ϴ� ������ �׸� 
        Handles.DrawWireDisc(fov.transform.position, //���� ��ǥ
                             Vector3.up,        //��� ����
                             fov.viewRange);    //���� ������ 
        //��ä���� ������ ����
        Handles.color = new Color(1, 1, 1, 0.2f);

        //ä���� ��ä���� �׸�
        Handles.DrawSolidArc(fov.transform.position, //������ǥ
                             Vector3.up,   // ��� ����
                             fromAnglePos, // ��ä���� ���� ��ǥ
                             fov.viewAngle, //��ä���� ����
                             fov.viewRange); //��ä���� ������ 

        //�þ߰��� �ؽ�Ʈ�� ǥ�� 
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f),
            fov.viewAngle.ToString());
    }
}