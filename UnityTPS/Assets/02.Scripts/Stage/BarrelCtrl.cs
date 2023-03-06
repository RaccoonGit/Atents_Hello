using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    string bulletTag = "BULLET";
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
    [SerializeField]
    private Rigidbody rb;

    public Texture[] textures;
    [SerializeField]
    private Material[] mats;
    [SerializeField]
    private MeshRenderer _renderer;

    [Header("메쉬")]
    public Mesh[] meshes;
    public MeshFilter filter;

    [SerializeField]
    private List<GameObject> streamFire;

    public int hitCount = 0;
    public bool isExplode = false;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        rb = GetComponent<Rigidbody>();
        _source = GetComponent<AudioSource>();
        explodeEff = Resources.Load<GameObject>("Effects/SmallExplosion_VFX");
        streamEff = Resources.Load<GameObject>("Effects/FlameStream_VFX");
        flameEff = Resources.Load<GameObject>("Effects/MediumFlames_VFX");
        explodeClip = Resources.Load<AudioClip>("Sound/grenade_exp2_SFX");
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        // _renderer.material = mats[Random.Range(0,3)];
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == bulletTag)
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

    public void Explode()
    {
        isExplode = true;
        streamFire.Clear();
        hitCount = 0;
        var expEff = Instantiate(explodeEff, transform.position, Quaternion.identity);
        Destroy(expEff, 1.0f);
        _source.PlayOneShot(explodeClip, 1.0f);

        // 자기 자신의 위치에서 20 근방에 있는 배럴의(콜라이더와 리지드 바디) 충돌체들을 cols 라는 배열에 대입한다.
        Collider[] cols = Physics.OverlapSphere(transform.position, 20.0f, 1 << LayerMask.NameToLayer("Barrel"));

        foreach(Collider col in cols)
        {
            if(!col.GetComponent<BarrelCtrl>().isExplode)
                col.GetComponent<BarrelCtrl>().Explode();

            if(rb != null)
            {
                rb.mass = 1.0f;
                // 리지드 바디 클래스에 있는 AddExplosionForce() 함수
                rb.AddExplosionForce(120.0f, transform.position, 20.0f, 100.0f);
            }
        }

        int idx = Random.Range(0, meshes.Length);
        filter.sharedMesh = meshes[idx];
        Invoke("BarrelMassBack", 3.0f);
        // Destroy(gameObject, 3.0f);
    }

    void BarrelMassBack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 20.0f, 1 << LayerMask.NameToLayer("Barrel"));

        foreach (Collider col in cols)
        {
            if (!col.GetComponent<BarrelCtrl>().isExplode)
                col.GetComponent<BarrelCtrl>().Explode();

            if (rb != null)
            {
                rb.mass = 50.0f;
                rb.AddExplosionForce(120.0f, transform.position, 20.0f, 100.0f);
            }
        }
    }
}