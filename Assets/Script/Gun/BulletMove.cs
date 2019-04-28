using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    
    public float speed;
    public float fireRate;
    public string BulletMode;
    private float p_speed;
    public Vector3 dir;
    private Vector3 pos;
    private Quaternion rot;
    public float Damage;
    private Rigidbody rb;
    
    private void Awake()
    {
        p_speed = speed;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        if(speed!=0)
            rb.velocity = dir * speed;
        else
            Debug.Log("No Speed"); 
    }
    private void OnCollisionEnter(Collision col)
    {
        speed = 0;
        rb.velocity =Vector3.zero;
        ContactPoint contact = col.contacts[0];
        rot = Quaternion.FromToRotation(Vector3.up,contact.normal);
        pos = contact.point;
        if(BulletMode!=null)
        {
            if(BulletMode =="Regular")
                HitSpawn("RegularHit");
            else if(BulletMode =="Desire")
                HitSpawn("DesireHit");
            else if(BulletMode =="Supreme")
                HitSpawn("SupremeHit");
        }
        

        StartCoroutine(ObjectPool.ReturnToPool(gameObject,0));
    }

    private void OnEnable()
    {
        StartCoroutine(ObjectPool.ReturnToPool(gameObject,3));
        speed = p_speed;
        rb.velocity =Vector3.zero;
    }
    private void HitSpawn(string hitVfxName)
    {
        var hitVfx = ObjectPool.TakeFormPool(hitVfxName);
        hitVfx.SetParent(null);
        hitVfx.transform.position = pos;
        hitVfx.rotation = rot;
    }

}
