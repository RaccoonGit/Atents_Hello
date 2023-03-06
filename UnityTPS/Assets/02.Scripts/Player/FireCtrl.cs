using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 총알
// 2. 발사 위치 포지션
// 3. 연사, 점사

[System.Serializable]
public struct PlayerSFX
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}


public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE = 0, SHOTGUN = 1, SNIPER = 2
    }

    public WeaponType curWeapon = WeaponType.RIFLE;

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
    #endregion

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
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(!playerCtrl.isRun)
            {
                if (Time.time - timePrev > 0.2f)
                {
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
    #endregion

    /***********************************************************************
    *                            Private Methods
    ***********************************************************************/
    #region Private Methods
    /// <summary> 총알 발사 : _bullet 오브젝트 생성하고 오디오 재생 </summary>
    private void Fire()
    {
        _source.PlayOneShot(m4SFX);
        GameObject obj = Instantiate(_bullet, firePos.position, firePos.rotation);
        muzzleFlash.Play();
    }
    #endregion
}
