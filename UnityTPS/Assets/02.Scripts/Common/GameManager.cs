using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;

// 1. 적 스폰 위치 랜덤하게
// 2. 적 프리펩
// 3. 적 스폰 레이트
// 4. 적 스폰 수
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

    [Header("오브젝트 풀")]
    [SerializeField]
    private List<GameObject> enemyPool = new List<GameObject>();
    [SerializeField]
    private List<GameObject> swatPool = new List<GameObject>();
    public static GameManager inst;
    // 다른 클래스랑 접근하기 위한 대표 변수 객체 생성은 한번만 되고 

    private bool isPause = false;

    public CanvasGroup inventoryCG;

    //[HideInInspector] public int KillCount = 0;
    [Header("게임 데이터")]
    private DataManager dataManager;

    public GameData gameData;

    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    private GameObject slotList;
    public GameObject[] itemObjects;

    public Text killCountText;

    private void Awake()
    {
        #region 싱글톤
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

        #region ScriptableObject로 저장하는 방식이 아닌 경우
        // Data Manager를 통해서 파일에 저장된 데이터 불러오기
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

        // 스폰 포인트가 있는 경우 반복
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

        // 스폰 포인트가 있는 경우 반복
        if (points.Count > 0)
        {
            // StartCoroutine(CreateEnemy());
            InvokeRepeating("CreateSWAT", 0.3f, 3.0f);
        }
    }

    private void CreateEnemy()
    {
        // 플레이어가 사망하면 코루틴 종료해 다음 루틴을 진행하지 않음
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
        // 플레이어가 사망하면 코루틴 종료해 다음 루틴을 진행하지 않음

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
        // 죽인 횟수를 저장
        // PlayerPrefs.SetInt("KILL_COUNT", KillCount);

        ++gameData.killCount;
        killCountText.text = "<color=#ff0000>KILL</color>" + "<color=#ffff00>" + gameData.killCount.ToString("0000") + "</color>";
        // dataManager.Save(gameData);
    }

    public void SaveGameData()
    {
        // dataManager.Save(gameData);

        // .asset 파일에 저장
        // UnityEditor.EditorUtility.SetDirty(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    void InventorySetup()
    {
        var slots = slotList.GetComponentsInChildren<Transform>();

        // 보유한 아이템의 갯수반큼 반복
        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            // 인벤토리 UI에 있는 Slot갯수만큼 반복
            for(int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0) continue;

                // 보유한 아이템 종류에 따라 인덱스를 추출
                int itemIndex = (int)gameData.equipItem[i].itemType;
                // 아이템의 부모를 Slot 게임 오브젝트로 변경
                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[j]);
                // 아이템의 ItemInfo 클래스의 itemData에 로드한 데이터 값을 저장
                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                // 아이템을 Slot에 추가하면 바깥 for구문으로 빠져나간다.
                break;
            }
        }
    }

    // 오브젝트 풀
    // 특히 모바일 플랫폼에서 게임 오브젝트 또는 프리펩을 동적으로 생성하는 작업은
    // 물리적한 부하가 걸릴 수 밖에 없다.
    // 따라서 주기적 또는 반복적으로 생성하는 객체는 씬이 로드할 때 모두 생성한 후
    // 사용하는 방식이 속도면에서 유리하다, 이처럼 미리 사용할 객체를 미리 만들어 놓은 후
    // 필요할 때마다 가져다 사용하는 방식을 오브젝트 풀링이라고 한다.
    //private IEnumerator CreateEnemy()
    //{
    //    while (!isGameOver)
    //    {
    //        yield return new WaitForSeconds(3.0f);
    //        if (isGameOver) yield break;
    //        // 플레이어가 사망하면 코루틴 종료해 다음 루틴을 진행하지 않음

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

    #region 스폰 함수
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

    #region Start Coroutine으로 스폰되기
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
