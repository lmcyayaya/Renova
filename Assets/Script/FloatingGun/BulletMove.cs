using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    
    public float speed;
    public float fireRate;
    private float p_speed;
    public Vector3 dir;
    private void Awake()
    {
        p_speed = speed;
    }
    void FixedUpdate()
    {
        
        if(speed!=0)
            this.GetComponent<Rigidbody>().velocity = dir *speed *Time.deltaTime;
        else
            Debug.Log("No Speed"); 
    }
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        speed = 0;
        this.GetComponent<Rigidbody>().velocity =Vector3.zero;
        ContactPoint contact = col.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up,contact.normal);
        Vector3 pos = contact.point;
        var hitVfx = ObjectPool.TakeFormPool("vfx_Hit");
        hitVfx.SetParent(null);
        hitVfx.transform.position = pos;
        hitVfx.rotation = rot;
        StartCoroutine(ObjectPool.ReturnToPool(gameObject,0));
        
        
    }

    private void OnEnable()
    {
        ReturnToPool(gameObject,3);
        speed = p_speed;
        this.GetComponent<Rigidbody>().velocity =Vector3.zero;
        
    }
    private void ReturnToPool(GameObject obj , float t)
    {
        StartCoroutine(ObjectPool.ReturnToPool(obj,t));
    }
}
