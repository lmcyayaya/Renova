using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    
    public float speed;
    public float fireRate;
    public GameObject  muzzlePrefab;
    public GameObject hitPrefab;
    float timer=0;
    private void Start()
    {
        if(muzzlePrefab!=null)
        {
            var muzzleVfx = Instantiate(muzzlePrefab,transform.position,Quaternion.identity);
            muzzleVfx.transform.forward = gameObject.transform.forward;

            var psMuzzle = muzzleVfx.GetComponent<ParticleSystem>();
            if(psMuzzle!=null)
            {   
                Destroy(muzzleVfx,psMuzzle.main.duration);
            }
            else
            {
                var psChild = muzzleVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVfx,psChild.main.duration);
            }
        }
    }
    void Update()
    {
        if(speed!=0)
            transform.position += transform.forward * (speed*Time.deltaTime);
        else
            Debug.Log("No Speed");

        timer+=Time.deltaTime;
        if(timer>6)
        {
            timer = 0;
            ObjectPool.ReturnToPool(this.gameObject);
        }
            
    }
    private void OnCollisionEnter(Collision col)
    {
        speed = 0;
        ContactPoint contact = col.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up,contact.normal);
        Vector3 pos = contact.point;
        if(hitPrefab!=null)
        {
            var hitVfx = Instantiate(hitPrefab,pos,rot);

            var psHit = hitVfx.GetComponent<ParticleSystem>();
            if(psHit!=null)
            {
                Destroy(hitVfx,psHit.main.duration);
            }
            else
            {
                var psChild = hitVfx.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVfx,psChild.main.duration);
            }
        }
        ObjectPool.ReturnToPool(this.gameObject);
    }
}
