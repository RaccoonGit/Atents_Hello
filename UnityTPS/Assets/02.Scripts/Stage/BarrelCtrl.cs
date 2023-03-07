using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    #region Tag String
    string bulletTag = "BULLET";
    string e_bulletTag = "E_BULLET";
    #endregion

    #region Components
    [SerializeField]
    private Rigidbody rbody;
    [SerializeField]
    private MeshRenderer _renderer;
    public MeshFilter _filter;
    #endregion

    #region Private Fields
    [SerializeField]
    private List<GameObject> streamFire;
    #endregion

    #region Public Fields
    public int hitCount = 0;
    public bool isExplode = false;
    #endregion

    #region Resources Load Objects
    [SerializeField]
    private GameObject explodeEff;
    [SerializeField]
    private GameObject streamEff;
    [SerializeField]
    private GameObject flameEff;
    [SerializeField]
    private AudioSource _source;
    [SerializeField]
    private AudioClip explodeClip;
    [SerializeField]
    private AudioClip streamClip;
    #endregion

    #region Editor Bind Objects
    [SerializeField]
    private Texture[] textures;
    [SerializeField]
    private Material[] mats;
    [Header("�޽�")]
    public Mesh[] meshes;
    #endregion

    /***********************************************************************
    *                             Unity Events
    ***********************************************************************/
    #region Unity Events
    /// <summary> ������Ʈ ���ε� </summary>
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _filter = GetComponent<MeshFilter>();
        rbody = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();

        explodeEff = Resources.Load<GameObject>("Effects/SmallExplosion_VFX");
        streamEff = Resources.Load<GameObject>("Effects/FlameStream_VFX");
        flameEff = Resources.Load<GameObject>("Effects/MediumFlames_VFX");
        explodeClip = Resources.Load<AudioClip>("Sound/grenade_exp2_SFX");

        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
    }

    /// <summary> �ݶ��̴� �浹 ���� �޼��� </summary>
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == bulletTag || col.collider.tag == e_bulletTag)
        {
            if (hitCount >= 4)
            {
                for(int i = 0; i < streamFire.Count; i++)
                {
                    Destroy(streamFire[i]);
                }
                Explode();
            }
            else
            {
                Vector3 hitPos = col.contacts[0].point;
                Quaternion hitRot = Quaternion.FromToRotation(Vector3.forward, hitPos.normalized);
                GameObject stream = Instantiate(streamEff, hitPos, hitRot, transform);
                streamFire.Add(stream);
                hitCount++;
                _source.PlayOneShot(streamClip);
            }
        }
    }
    #endregion

    /***********************************************************************
    *                            Private Methods
    ***********************************************************************/
    #region Private Methods
    private void BarrelMassBack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 20.0f, 1 << LayerMask.NameToLayer("Barrel"));

        foreach (Collider col in cols)
        {
            if (!col.GetComponent<BarrelCtrl>().isExplode)
                col.GetComponent<BarrelCtrl>().Explode();

            if (rbody != null)
            {
                rbody.mass = 50.0f;
                rbody.AddExplosionForce(120.0f, transform.position, 20.0f, 100.0f);
            }
        }
    }
    #endregion

    /***********************************************************************
    *                            Public Methods
    ***********************************************************************/
    #region Public Methods
    public void Explode()
    {
        isExplode = true;
        streamFire.Clear();
        hitCount = 0;
        var expEff = Instantiate(explodeEff, transform.position, Quaternion.identity);
        Destroy(expEff, 1.0f);
        _source.PlayOneShot(explodeClip, 1.0f);

        // �ڱ� �ڽ��� ��ġ���� 20 �ٹ濡 �ִ� �跲��(�ݶ��̴��� ������ �ٵ�) �浹ü���� cols ��� �迭�� �����Ѵ�.
        Collider[] cols = Physics.OverlapSphere(transform.position, 20.0f, 1 << LayerMask.NameToLayer("Barrel"));

        foreach (Collider col in cols)
        {
            if (!col.GetComponent<BarrelCtrl>().isExplode)
                col.GetComponent<BarrelCtrl>().Explode();

            if (rbody != null)
            {
                rbody.mass = 1.0f;
                // ������ �ٵ� Ŭ������ �ִ� AddExplosionForce() �Լ�
                rbody.AddExplosionForce(120.0f, transform.position, 20.0f, 100.0f);
            }
        }

        int idx = Random.Range(0, meshes.Length);
        _filter.sharedMesh = meshes[idx];
        Invoke("BarrelMassBack", 3.0f);
        // Destroy(gameObject, 3.0f);
    }
    #endregion
}