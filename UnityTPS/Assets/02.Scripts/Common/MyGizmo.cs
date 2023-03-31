using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    #region Public Field
    public enum Type { NORMAL, WAYPOINT}
    private const string wayPointFile = "Enemy";
    [SerializeField]
    public Type type = Type.WAYPOINT;

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
        if(type == Type.NORMAL)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
        }
        else if (type == Type.WAYPOINT)
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, wayPointFile, true);
            Gizmos.DrawSphere(transform.position, _radius);
        }
    }
    #endregion
}
