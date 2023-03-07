using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    #region Public Field
    // 기즈모 색상
    public Color _color = Color.green;
    // 기즈모 반경
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
