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

    [SerializeField]
    private PlayerCtrl playerCtrl;

    public PlayerSFX playerSFX;

    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private Transform firePos;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private AudioSource _source;
    [SerializeField]
    private AudioClip m4SFX;

    float timePrev;
    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        _bullet = Resources.Load<GameObject>("Bullet");
        _source = GetComponent<AudioSource>();
        m4SFX = Resources.Load<AudioClip>("Sound/p_m4_SFX");
        muzzleFlash = GameObject.Find("FirePos").GetComponentInChildren<ParticleSystem>();
        muzzleFlash.Stop();
        timePrev = Time.time;
        // 싱글 게임이면 아래 방식
        // firePos = GameObject.Find("FirePos").transform;
    }

    // Update is called once per frame
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

    private void Fire()
    {
        _source.PlayOneShot(m4SFX);
        GameObject obj = Instantiate(_bullet, firePos.position, firePos.rotation);
        muzzleFlash.Play();
    }
}
