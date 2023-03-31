using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager poolManager;
    [Header("Object Pool")]
    [SerializeField]
    private GameObject bulletPrefab = null;
    [SerializeField]
    private List<GameObject> bulletPool = new List<GameObject>();

    private float maxCount = 10;
    void Awake()
    {
        if (poolManager == null)
            poolManager = this;
        else if (poolManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        bulletPrefab = Resources.Load<GameObject>("Bullet");
        //CreatePooling();
    }

    public void CreatePooling()
    {
        GameObject bulletParents = new GameObject("Bullet Parents");
        for(int i = 0; i < maxCount; i++)
        {
            var bulletObj = Instantiate(bulletPrefab, bulletParents.transform);
            bulletObj.name = "Bullet_" + (i + 1).ToString();
            bulletObj.SetActive(false);
            bulletPool.Add(bulletObj);
        }
    }

    public GameObject GetBullet()
    {
        for(int i =0; i < bulletPool.Count; i++)
        {
            if(bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }
}
