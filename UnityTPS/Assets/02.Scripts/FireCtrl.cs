using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. �Ѿ�
// 2. �߻� ��ġ ������
// 3. ����, ����

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
        // �̱� �����̸� �Ʒ� ���
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
