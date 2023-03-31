using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 1. 총알
// 2. 발사 위치 포지션
// 3. 연사, 점사
[System.Serializable]
public struct PlayerSFX
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE = 0, SHOTGUN = 1, SNIPER = 2
    }

    public WeaponType curWeaponType = WeaponType.RIFLE;

    #region Components
    private AudioSource _source;
    #endregion

    #region Classes
    [SerializeField]
    private PlayerCtrl playerCtrl;
    #endregion

    #region Struct
    public PlayerSFX playerSFX;
    #endregion

    #region Resources Objects
    private GameObject _bullet;
    private ParticleSystem muzzleFlash;
    private AudioClip m4SFX;
    #endregion

    #region Private Fields
    [SerializeField]
    private Transform firePos;
    private float timePrev;
    private float randomTime;

    private int maxBullet = 10; // 최대 총알 수
    private int curBullet = 10; // 남은 총알 수
    private float reloadTime = 2.0f;
    public bool isReload = false;

    [Header("탄창 UI")]
    private string magaineUI = "Panel-Magazine";
    [SerializeField]
    private Text magazineText;
    [SerializeField]
    private Image magazineImgOff;
    [SerializeField]
    private Image magazineImg;
    #endregion

    public Sprite[] weaponIcons;
    public Image weaponImage;
    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> 컴포넌트 바인딩 </summary>
    void Awake()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        _source = GetComponent<AudioSource>();

        _bullet = Resources.Load<GameObject>("Bullet");
        m4SFX = Resources.Load<AudioClip>("Sound/p_m4_SFX");
        muzzleFlash = GameObject.Find("FirePos").GetComponentInChildren<ParticleSystem>();
        muzzleFlash.Stop();

        // 이전 시간 초기화
        timePrev = Time.time;
        // 싱글 게임이면 아래 방식
        // firePos = GameObject.Find("FirePos").transform;
        magazineText = GameObject.Find(magaineUI).transform.GetChild(0).GetComponent<Text>();
        magazineImgOff = GameObject.Find(magaineUI).transform.GetChild(1).GetComponent<Image>();
        magazineImg = GameObject.Find(magaineUI).transform.GetChild(2).GetComponent<Image>();

        UpdateBullet();

    }

    void Update()
    {
        // 마우스 포인트가 버튼 위에 올려져 있다면 빠져나간다
        #region 마우스가 버튼을 클릭 했을 때 발사를 막는 첫번째 방법
        // if (EventSystem.current.IsPointerOverGameObject()) return;
        #endregion

        #region 두번째 방법
        if (MouseHover.mouseHover.isHover) return;
        if (Drag.draggingItem != null) return;
        #endregion
        Debug.DrawRay(firePos.position, firePos.forward * 15.0f, Color.green);

        if (Input.GetMouseButton(0) && !isReload)
        {
            if (!playerCtrl.isRun)
            {
                randomTime = Random.Range(0.15f, 0.25f);
                if (Time.time - timePrev > randomTime)
                {
                    ReloadigRoutine();
                    Fire();
                    timePrev = Time.time;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && !isReload)
        {
            if (!playerCtrl.isRun)
            {
                randomTime = Random.Range(0.15f, 0.25f);
                if (Time.time - timePrev > randomTime)
                {
                    ReloadigRoutine();
                    Fire();
                    timePrev = Time.time;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
        }
    }

    private void ReloadigRoutine()
    {
        --curBullet;
        if (curBullet == 0)
        {
            StartCoroutine(Reloading());
        }
    }
    #endregion

    /***********************************************************************
    *                            Private Methods
    ***********************************************************************/
    #region Private Methods
    /// <summary> 총알 발사 : _bullet 오브젝트 생성하고 오디오 재생 </summary>
    private void Fire()
    {
        //var _bullet = PoolingManager.poolManager.GetBullet();
        //if(_bullet != null)
        //{
        //    _bullet.transform.position = firePos.position;
        //    _bullet.transform.rotation = firePos.rotation;
        //    _bullet.SetActive(true);
        //}
        //// GameObject obj = Instantiate(_bullet, firePos.position, firePos.rotation);

        RaycastHit hit;
        if(Physics.Raycast(firePos.position, firePos.forward, out hit))
        {
            if (hit.collider.tag == "ENEMY")
            {
                // 모든 자료형의 최상위 부모 클래스 객체
                object[] _param = new object[2];
                _param[0] = hit.point;
                _param[1] = 20.0f;
                hit.collider.gameObject.SendMessage("OnDamage", _param, SendMessageOptions.DontRequireReceiver);
            }
            if (hit.collider.tag == "S_ENEMY")
            {
                // 모든 자료형의 최상위 부모 클래스 객체
                object[] _param = new object[2];
                _param[0] = hit.point;
                _param[1] = 20.0f;
                hit.collider.gameObject.SendMessage("OnDamage", _param, SendMessageOptions.DontRequireReceiver);
            }
            if (hit.collider.tag == "WALL")
            {
                // 모든 자료형의 최상위 부모 클래스 객체
                object[] _param = new object[2];
                _param[0] = hit.point;
                _param[1] = 20.0f;
                hit.collider.gameObject.SendMessage("OnDamage", _param, SendMessageOptions.DontRequireReceiver);
            }
            if (hit.collider.tag == "BARREL")
            {
                // 모든 자료형의 최상위 부모 클래스 객체
                object[] _param = new object[2];
                _param[0] = hit.point;
                _param[1] = firePos.position;
                hit.collider.gameObject.SendMessage("OnDamage", _param, SendMessageOptions.DontRequireReceiver);
            }
        }
        _source.PlayOneShot(m4SFX);
        if(!isReload)
            muzzleFlash.Play();
        else
            muzzleFlash.Stop();
        UpdateBullet();
    }

    void UpdateBullet()
    {
        magazineText.text = string.Format("<color=#ffff00>{0}</color> / {1}", curBullet, maxBullet);
        magazineImg.fillAmount = (float)curBullet / (float)maxBullet;
    }

    public void OnChangeWeapon()
    {
        curWeaponType = (WeaponType)((int)++curWeaponType % 2);
        weaponImage.sprite = weaponIcons[(int)curWeaponType];
    }
    #endregion

    private IEnumerator Reloading()
    {
        isReload = true;
        muzzleFlash.Stop();
        _source.PlayOneShot(playerSFX.reload[(int)curWeaponType]);
        yield return new WaitForSeconds(reloadTime);
        isReload = false;
        magazineImg.fillAmount = 1.0f;
        curBullet = maxBullet;
        UpdateBullet();
    }
}
