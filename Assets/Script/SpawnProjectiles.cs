﻿using System.Collections;
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
        GameObject vfx;
        if(firePoint!=null)
        {
            vfx = Instantiate(effectToSpawn,firePoint.transform.position,Quaternion.identity);
            if(rotateGun!=null)
            {
                vfx.transform.localRotation = rotateGun.GetRotation();
            }
        }
        else
        {
            Debug.Log("No Fire Point");
        }

    }
}
