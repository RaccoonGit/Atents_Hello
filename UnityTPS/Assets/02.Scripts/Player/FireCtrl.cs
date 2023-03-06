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
    /// <summary> ������Ʈ ���ε� </summary>
    void Awake()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        _source = GetComponent<AudioSource>();

        _bullet = Resources.Load<GameObject>("Bullet");
        m4SFX = Resources.Load<AudioClip>("Sound/p_m4_SFX");
        muzzleFlash = GameObject.Find("FirePos").GetComponentInChildren<ParticleSystem>();
        muzzleFlash.Stop();

        // ���� �ð� �ʱ�ȭ
        timePrev = Time.time;
        // �̱� �����̸� �Ʒ� ���
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
    /// <summary> �Ѿ� �߻� : _bullet ������Ʈ �����ϰ� ����� ��� </summary>
    private void Fire()
    {
        _source.PlayOneShot(m4SFX);
        GameObject obj = Instantiate(_bullet, firePos.position, firePos.rotation);
        muzzleFlash.Play();
    }
    #endregion
}
