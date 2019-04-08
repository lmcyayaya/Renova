using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnProjectiles : MonoBehaviour
{
    
    public GameObject firePoint;
    public int shootCount = 0;
    public float damagePlus =1;
    public float chargeTime;
    public float chargeLevel = 1;
    private StateManager state;
    private BaseData baseData;
    private ProcessedData processedData;
    private RotateGun rotateGun;
    private ATKManager atkManager;
    private float timeToFire;
    void Start()
    {
        atkManager = GameObject.FindWithTag("GM").GetComponent<ATKManager>();
        state = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
        baseData = state.baseData;
        processedData = state.processedData;
        rotateGun = this.gameObject.GetComponent<RotateGun>();
    }
    void Update()
    {
        Reset();
        if(baseData.atkModeData.modeName == "Regular")
            RegularMode();
        else if (baseData.atkModeData.modeName == "Desire")
            DesireMode();
        else if(baseData.atkModeData.modeName == "Supreme")
            SupremeMode();
    }
    public void SpawnBullet(string bulletName,string muzzleName)
    {
        StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.05f,0.03f,1.5f));
        Transform vfx = null ;
        if(firePoint!=null)
        {
            //vfx = Instantiate(effectToSpawn,firePoint.transform.position,Quaternion.identity);
            vfx = ObjectPool.TakeFormPool(bulletName);
            vfx.SetParent(null);
            vfx.transform.position = firePoint.transform.position;
            if(rotateGun!=null)
            {
                vfx.transform.localRotation = rotateGun.GetRotation();
                vfx.GetComponent<BulletMove>().dir = vfx.transform.forward;
            }
            atkManager.CalculateATK();
            vfx.GetComponent<BulletMove>().Damage = processedData.ATK;
        }
        else
        {
            Debug.Log("No Fire Point");
        }
        var muzzleVfx = ObjectPool.TakeFormPool(muzzleName);
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

    void RegularMode()
    {
        if(state.r2 && Time.time >= timeToFire)
        {   
            SpawnBullet("RegularBullet","RegularMuzzle");
            timeToFire = Time.time + baseData.atkModeData.fireRate;
        } 
    }
    void DesireMode()
    {
        if(state.r2 && Time.time >= timeToFire)
        {   
            if(shootCount ==5)
            {
                damagePlus=1.5f;
                shootCount = 0;
            }
            SpawnBullet("DesireBullet","DesireMuzzle");
            timeToFire = Time.time + baseData.atkModeData.fireRate;

            damagePlus = 1;
            shootCount +=1;
                
        } 
    }
    void SupremeMode()
    {
        if(!state.r2 && chargeTime>=1 && Time.time > timeToFire)
        {   
            SpawnBullet("SupremeBullet","SupremeMuzzle");
            timeToFire = Time.time + baseData.atkModeData.fireRate;
            chargeTime = 0;
            chargeLevel = 1;
        }
        else if(state.r2)
        {
            chargeTime +=Time.deltaTime;
            if(chargeTime >=3)
            {
                chargeTime=3;
                chargeLevel = 3;
            }
            else if(chargeTime>=2)
                chargeLevel = 2;
                
        }
    }
    private void Reset()
    {

        if(baseData.changeMode)
        {
            shootCount = 0;
        }
    }
}
