using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_EnemyFire : MonoBehaviour
{
    #region Animator Hash
    private readonly int hashFire = Animator.StringToHash("isFire");
    private readonly int hashReload = Animator.StringToHash("isReload");
    #endregion

    #region Components
    private Animator _animator;
    private AudioSource _source;
    private MeshRenderer _muzzleFlash;
    #endregion

    #region Classes
    FirePosCtrl firePosCtrl;
    #endregion

    #region Private Field
    [Header("Target Rotation")]
    private Transform playerTr;
    private Transform enemyTr;
    private Transform firePos;

    private float nextTime = 0.0f;
    private float fireRate = 0.4f;
    private float damping = 10.0f;

    [SerializeField]
    [Tooltip("재장전 시간")]
    private readonly float reloadTime = 2.0f;
    [SerializeField]
    [Tooltip("최대 총알 수")]
    private readonly int maxAmmo = 20;

    [SerializeField]
    [Tooltip("현재 총알 수")]
    private int curAmmo = 20;
    private WaitForSeconds wsReload;
    #endregion

    #region Public Field
    public bool isFire = false;
    public bool isReload = false;
    #endregion

    #region Resources Objects
    private GameObject _bulletPrefab;
    private AudioClip fireSfx;
    private AudioClip reloadSfx;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
        firePosCtrl = GetComponentInChildren<FirePosCtrl>();

        playerTr = GameObject.FindWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        firePos = transform.GetChild(3).GetChild(0).GetChild(1).GetComponent<Transform>();
        _muzzleFlash = transform.GetChild(3).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>();

        _bulletPrefab = Resources.Load<GameObject>("E_Bullet");
        fireSfx = Resources.Load<AudioClip>("Sound/p_ak_SFX");
        reloadSfx = Resources.Load<AudioClip>("Sound/p_reload_1");

        wsReload = new WaitForSeconds(reloadTime);

        _muzzleFlash.enabled = false;
    }

    void Update()
    {
        if (isFire && !isReload)
        {
            if (Time.time >= nextTime)
            {
                Fire();
                nextTime = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * damping);
            Vector3 upperVec = new Vector3(0.0f, 1.5f, 0.0f);
            Quaternion fireRot = Quaternion.LookRotation((playerTr.position + upperVec) - firePos.position);
            firePos.rotation = Quaternion.Slerp(firePos.rotation, fireRot, Time.deltaTime * damping);
            // firePosCtrl.SetFirePosRotation(fireRot);
        }
    }
    #endregion

    /***********************************************************************
    *                           Priavte Methods
    ***********************************************************************/
    #region Priavte Methods
    private void Fire()
    {
        var bullet = Instantiate(_bulletPrefab, firePos.position, firePos.rotation);
        Destroy(bullet, 3.0f);
        _source.PlayOneShot(fireSfx);
        _animator.SetTrigger(hashFire);
        isReload = (--curAmmo % maxAmmo == 0);
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
        StartCoroutine(ShowMuzzleFlash());
    }
    #endregion

    private IEnumerator ShowMuzzleFlash()
    {
        float _scale = Random.Range(1.0f, 2.0f);
        _muzzleFlash.transform.localScale = Vector3.one * _scale;
        _muzzleFlash.enabled = true;
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        _muzzleFlash.transform.localRotation = rot;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        _muzzleFlash.enabled = false;
    }

    private IEnumerator Reloading()
    {
        _animator.SetTrigger(hashReload);
        _source.PlayOneShot(reloadSfx, 1.0f);
        yield return wsReload;
        curAmmo = maxAmmo;
        isReload = false;
    }
}
