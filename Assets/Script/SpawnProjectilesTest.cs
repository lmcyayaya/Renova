using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
public class SpawnProjectilesTest : MonoBehaviour
{
    
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public RotateGunTest rotateGunTest;

    private GameObject effectToSpawn;
    private float timeToFire;
    void Start()
    {
        effectToSpawn = vfx[0]; 
    }
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1/ effectToSpawn.GetComponent<BulletMove>().fireRate;
            SpawnBullet();
        }
            
    }
    public void SpawnBullet()
    {
        GameObject vfx;
        if(firePoint!=null)
        {
            vfx = Instantiate(effectToSpawn,firePoint.transform.position,Quaternion.identity);
            if(rotateGunTest!=null)
            {
                vfx.transform.localRotation = rotateGunTest.GetRotation();
            }
        }
        else
        {
            Debug.Log("No Fire Point");
        }

    }
}
