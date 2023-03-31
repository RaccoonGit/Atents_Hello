using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 오리지널 값
    private Vector3 posCamera;
    private Quaternion rotCamera;

    private Transform tr;

    public bool isShake = false;
    [SerializeField]
    private float timeShake;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isShake)
        {
            float x = Random.Range(-0.2f, 0.2f);
            float y = Random.Range(-0.2f, 0.2f);
            float z = 0.0f;
            tr.position += new Vector3(x, y, z);

            Quaternion rot = Quaternion.Euler(x, y, z);
            tr.rotation = rot;

            if(Time.time - timeShake > 0.3f)
            {
                isShake = false;
                tr.position = posCamera;
                tr.rotation = rotCamera;
            }
        }
    }

    public void TurnOnShake()
    {
        if(isShake == false)
        {
            isShake = true;
        }

        posCamera = tr.position;
        rotCamera = tr.rotation;
        timeShake = Time.time;
    }
}
