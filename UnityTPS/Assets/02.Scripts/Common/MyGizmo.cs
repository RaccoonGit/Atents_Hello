using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    #region Public Field
    // ����� ����
    public Color _color = Color.green;
    // ����� �ݰ�
    public float _radius = 0.1f;
    #endregion

    /***********************************************************************
    *                            Gizmos Events
    ***********************************************************************/
    #region Gizmos Events
    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }
    #endregion
}
