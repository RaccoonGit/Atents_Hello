using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;

// 1. �� ���� ��ġ �����ϰ�
// 2. �� ������
// 3. �� ���� ����Ʈ
// 4. �� ���� ��
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private List<Transform> points;

    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _S_Enemy;

    public bool isGameOver = false;

    [SerializeField]
    private float spawnTime = 3.0f;

    [SerializeField]
    private int maxCount = 25;
    [SerializeField]
    private int enemyCount = 15;
    [SerializeField]
    private int swatCount = 10;

    [Header("������Ʈ Ǯ")]
    [SerializeField]
    private List<GameObject> enemyPool = new List<GameObject>();
    [SerializeField]
    private List<GameObject> swatPool = new List<GameObject>();
    public static GameManager inst;
    // �ٸ� Ŭ������ �����ϱ� ���� ��ǥ ���� ��ü ������ �ѹ��� �ǰ� 

    private bool isPause = false;

    public CanvasGroup inventoryCG;

    //[HideInInspector] public int KillCount = 0;
    [Header("���� ������")]
    private DataManager dataManager;

    public GameData gameData;

    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    private GameObject slotList;
    public GameObject[] itemObjects;

    public Text killCountText;

    private void Awake()
    {
        #region �̱���
        if (inst == null)
            inst = this;
        else if (inst != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        #endregion

        dataManager = GetComponent<DataManager>();
        dataManager.Initialized();

        if (spawnPoint != null)
        {
            spawnPoint.GetComponentsInChildren<Transform>(points);
            points.RemoveAt(0);
        }
        if(_enemy == null)
        {
            _enemy = Resources.Load<GameObject>("Enemy");
        }
        if(_S_Enemy == null)
        {
            _S_Enemy = Resources.Load<GameObject>("S_Enemy");
        }
        //StartCoroutine(CreateSpawn());
        //InvokeRepeating("SpawnSwat", 0.3f, 4.0f);
        // StartCoroutine(CreateEnemy());
        // InvokeRepeating("CreateEnemy", 0.3f, 3.0f);
    }

    private void Start()
    {
        CreatePooling();
        CreateSwatPooling();
        OnInventoryOpen(false);
        LoadGameData();
    }

    private void LoadGameData()
    {
        // KillCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        // killCountText.text = "<color=#ff0000>KILL</color>" + "<color=#ffff00>" + KillCount.ToString("0000") + "</color>";

        #region ScriptableObject�� �����ϴ� ����� �ƴ� ���
        // Data Manager�� ���ؼ� ���Ͽ� ����� ������ �ҷ�����
        //GameData data = dataManager.Load();
        //gameData.hp = data.hp;
        //gameData.damage = data.damage;
        //gameData.speed = data.speed;
        //gameData.killCount = data.killCount;
        //gameData.equipItem = data.equipItem;
        #endregion

        if (gameData.equipItem.Count > 0)
        {
            InventorySetup();
        }

        killCountText.text = "<color=#ff0000>KILL</color>" + "<color=#ffff00>" + gameData.killCount.ToString("0000") + "</color>";
    }

    private void CreatePooling()
    {
        GameObject objectPool = new GameObject("ObjectPool");
        for (int i = 0; i < enemyCount; i++)
        {
            var ene = Instantiate(_enemy, objectPool.transform);
            ene.name = "Enemy_" + (i + 1).ToString();
            ene.SetActive(false);
            enemyPool.Add(ene);
        }

        // ���� ����Ʈ�� �ִ� ��� �ݺ�
        if (points.Count > 0)
        {
            // StartCoroutine(CreateEnemy());
            InvokeRepeating("CreateEnemy", 0.3f, 3.0f);
        }
    }

    private void CreateSwatPooling()
    {
        GameObject objectPool = new GameObject("ObjectPool_SWAT");
        for (int i = 0; i < swatCount; i++)
        {
            var ene = Instantiate(_S_Enemy, objectPool.transform);
            ene.name = "Enemy_SWAT_" + (i + 1).ToString();
            ene.SetActive(false);
            swatPool.Add(ene);
        }

        // ���� ����Ʈ�� �ִ� ��� �ݺ�
        if (points.Count > 0)
        {
            // StartCoroutine(CreateEnemy());
            InvokeRepeating("CreateSWAT", 0.3f, 3.0f);
        }
    }

    private void CreateEnemy()
    {
        // �÷��̾ ����ϸ� �ڷ�ƾ ������ ���� ��ƾ�� �������� ����
        foreach (GameObject enemy in enemyPool)
        {
            if (enemy.activeSelf == false)
            {
                int idx = Random.Range(0, points.Count);
                enemy.transform.position = points[idx].position;
                enemy.SetActive(true);
                break;
            }
        }
    }

    private void CreateSWAT()
    {
        // �÷��̾ ����ϸ� �ڷ�ƾ ������ ���� ��ƾ�� �������� ����

        foreach (GameObject enemy in swatPool)
        {
            if (enemy.activeSelf == false)
            {
                int idx = Random.Range(0, points.Count);
                enemy.transform.position = points[idx].position;
                enemy.SetActive(true);
                break;
            }
        }
    }
    public void OnPauseClick()
    {
        isPause = !isPause;
        Time.timeScale = (isPause) ? 0.0f : 1.0f;
        var PlayerObj = GameObject.FindGameObjectWithTag("Player");
        var PlayerScripts = PlayerObj.GetComponents<MonoBehaviour>();
        foreach (var script in PlayerScripts)
        {
            script.enabled = !isPause;
        }
        var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();

        canvasGroup.blocksRaycasts = !isPause;
    }

    public void OnInventoryOpen(bool isOpened)
    {
        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;
        inventoryCG.interactable = (isOpened) ? true : false;
        inventoryCG.blocksRaycasts = (isOpened) ? true : false;
    }

    public void IncKillCount()
    {
        // ++KillCount;
        // killCountText.text = "<color=#ff0000>KILL</color>" + "<color=#ffff00>" + KillCount.ToString("0000") + "</color>";
        // ���� Ƚ���� ����
        // PlayerPrefs.SetInt("KILL_COUNT", KillCount);

        ++gameData.killCount;
        killCountText.text = "<color=#ff0000>KILL</color>" + "<color=#ffff00>" + gameData.killCount.ToString("0000") + "</color>";
        // dataManager.Save(gameData);
    }

    public void SaveGameData()
    {
        // dataManager.Save(gameData);

        // .asset ���Ͽ� ����
        // UnityEditor.EditorUtility.SetDirty(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    void InventorySetup()
    {
        var slots = slotList.GetComponentsInChildren<Transform>();

        // ������ �������� ������ŭ �ݺ�
        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            // �κ��丮 UI�� �ִ� Slot������ŭ �ݺ�
            for(int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0) continue;

                // ������ ������ ������ ���� �ε����� ����
                int itemIndex = (int)gameData.equipItem[i].itemType;
                // �������� �θ� Slot ���� ������Ʈ�� ����
                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[j]);
                // �������� ItemInfo Ŭ������ itemData�� �ε��� ������ ���� ����
                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                // �������� Slot�� �߰��ϸ� �ٱ� for�������� ����������.
                break;
            }
        }
    }

    // ������Ʈ Ǯ
    // Ư�� ����� �÷������� ���� ������Ʈ �Ǵ� �������� �������� �����ϴ� �۾���
    // �������� ���ϰ� �ɸ� �� �ۿ� ����.
    // ���� �ֱ��� �Ǵ� �ݺ������� �����ϴ� ��ü�� ���� �ε��� �� ��� ������ ��
    // ����ϴ� ����� �ӵ��鿡�� �����ϴ�, ��ó�� �̸� ����� ��ü�� �̸� ����� ���� ��
    // �ʿ��� ������ ������ ����ϴ� ����� ������Ʈ Ǯ���̶�� �Ѵ�.
    //private IEnumerator CreateEnemy()
    //{
    //    while (!isGameOver)
    //    {
    //        yield return new WaitForSeconds(3.0f);
    //        if (isGameOver) yield break;
    //        // �÷��̾ ����ϸ� �ڷ�ƾ ������ ���� ��ƾ�� �������� ����

    //        foreach(GameObject enemy in enemyPool)
    //        {
    //            if(enemy.activeSelf == false)
    //            {
    //                int idx = Random.Range(0, points.Count);
    //                enemy.transform.position = points[idx].position;
    //                enemy.SetActive(true);
    //                break;
    //            }
    //        }
    //    }
    //}

    #region ���� �Լ�
    //private IEnumerator CreateSpawn()
    //{
    //    int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;
    //    while (enemyCount < maxCount)
    //    {
    //        int spawnNum = Random.Range(0, 2);
    //        yield return new WaitForSeconds(spawnTime);
    //        int idx = Random.Range(0, points.Count);
    //        Instantiate(_enemy, points[idx].position, points[idx].rotation);
    //    }
    //}

    //void SpawnSwat()
    //{
    //    int swatEnemyCount = (int)GameObject.FindGameObjectsWithTag("S_ENEMY").Length;
    //    if(swatEnemyCount < maxCount)
    //    {
    //        int idx = Random.Range(0, points.Count);
    //        Instantiate(_S_Enemy, points[idx].position, points[idx].rotation);
    //    }
    //}
    #endregion

    #region Start Coroutine���� �����Ǳ�
    //private IEnumerator Spawn(float time, int count)
    //{
    //    float spawnCount = count;
    //    while(spawnCount > 0 && isGameOver)
    //    {
    //        GameObject obj = Instantiate(_enemy, points[Random.Range(0, points.Count)].position, Quaternion.identity);
    //        MoveAgent enemyAgent = obj.GetComponent<MoveAgent>();

    //        enemyAgent.nextIdx = Random.Range(0, enemyAgent.wayPoints.Count);
    //        spawnCount--;
    //        yield return new WaitForSeconds(time);
    //    }
    //}
    #endregion
}
