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
    public GameObject UI_Charge;
    public GameObject UI_ShootCount;
    private StateManager state;
    private ProcessedData processedData;
    private RotateGun rotateGun;
    private ATKManager atkManager;
    private float timeToFire;
    public void Init(StateManager stat)
    {

        state = stat;
        //processedData = state.processedData;
        atkManager = BaseData.Instance.gameObject.GetComponent<ATKManager>();
        rotateGun = this.gameObject.GetComponent<RotateGun>();
    }
    void Update()
    {
        Reset();
        if(BaseData.Instance.atkModeData.modeName == "Regular")
            RegularMode();
        else if (BaseData.Instance.atkModeData.modeName == "Desire")
            DesireMode();
        else if(BaseData.Instance.atkModeData.modeName == "Supreme")
            SupremeMode();
        UI_Charge.SetActive(BaseData.Instance.atkModeData.modeName == "Supreme");
        UI_ShootCount.SetActive(BaseData.Instance.atkModeData.modeName == "Desire");
    }
    public void SpawnBullet(string bulletName,string muzzleName)
    {
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
            vfx.GetComponent<BulletMove>().Damage = ProcessedData.Instance.ATK;
            if(BaseData.Instance.atkModeData.modeName =="Supreme")
            {
                var Child = vfx.GetChild(0);
                Child.transform.localScale = new Vector3(chargeLevel*0.5f,chargeLevel*0.5f,chargeLevel*0.5f);
                for(int i = 0; i< Child.childCount;i++)
                {
                    Child.GetChild(i).transform.localScale = new Vector3(chargeLevel*0.5f,chargeLevel*0.5f,chargeLevel*0.5f);
                }
            }
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
            StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.05f,0.03f,1.5f));
            SpawnBullet("RegularBullet","RegularMuzzle");
            timeToFire = Time.time + BaseData.Instance.atkModeData.fireRate;
        } 
    }
    void DesireMode()
    {
        if(state.r2 && Time.time >= timeToFire)
        {   
            
            StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.15f,0.1f,1f));
            if(shootCount ==5)
            {
                damagePlus=1.5f;
                shootCount = 0;
            }
            SpawnBullet("DesireBullet","DesireMuzzle");
            timeToFire = Time.time + BaseData.Instance.atkModeData.fireRate;

            damagePlus = 1;
            shootCount +=1;
                
        } 
    }
    void SupremeMode()
    {
        if(!state.r2 && chargeTime>=1 && Time.time > timeToFire)
        {   
            StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.1f*chargeLevel,0.1f,1.5f));
            SpawnBullet("SupremeBullet","SupremeMuzzle");
            timeToFire = Time.time + BaseData.Instance.atkModeData.fireRate;
            chargeTime = 0;
            chargeLevel = 1;
        }
        else if(!state.r2 && chargeTime<1)
        {
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
        if(BaseData.Instance.changeMode)
        {
            chargeTime = 0;
            shootCount = 0;
        }

    }
}
