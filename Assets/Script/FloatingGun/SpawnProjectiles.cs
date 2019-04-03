using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
public class SpawnProjectiles : MonoBehaviour
{
    
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public RotateGun rotateGun;
    public InputHandler inputHandler;

    private GameObject effectToSpawn;
    private float timeToFire;
    void Start()
    {
        effectToSpawn = vfx[0]; 
    }
    void Update()
    {
        if(inputHandler.r2_input && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1/ effectToSpawn.GetComponent<BulletMove>().fireRate;
            SpawnBullet();
        }
            
    }
    public void SpawnBullet()
    {
        StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.05f,0.03f,1.5f));
        Transform vfx = null ;
        if(firePoint!=null)
        {
            //vfx = Instantiate(effectToSpawn,firePoint.transform.position,Quaternion.identity);
            vfx = ObjectPool.TakeFormPool("Bullet");
            vfx.SetParent(null);
            vfx.transform.position = firePoint.transform.position;
            if(rotateGun!=null)
            {
                vfx.transform.localRotation = rotateGun.GetRotation();
                vfx.GetComponent<BulletMove>().dir = vfx.transform.forward;
            }
        }
        else
        {
            Debug.Log("No Fire Point");
        }
        var muzzleVfx = ObjectPool.TakeFormPool("vfx_muzzle");
        muzzleVfx.SetParent(null);
        muzzleVfx.transform.position = vfx.transform.position;
        muzzleVfx.transform.forward = vfx.transform.forward;

        var psMuzzle = muzzleVfx.GetComponent<ParticleSystem>();
        if(psMuzzle!=null)
        {   
            StartCoroutine(ObjectPool.ReturnToPool(muzzleVfx.gameObject,psMuzzle.main.duration));
        }
        else
        {
            var psChild = muzzleVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
            StartCoroutine(ObjectPool.ReturnToPool(muzzleVfx.gameObject,psChild.main.duration));
        }
    }
}
