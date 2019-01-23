using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    
    public float speed;
    public float fireRate;
    void Update()
    {
        if(speed!=0)
            transform.position += transform.forward * (speed*Time.deltaTime);
        else
            Debug.Log("No Speed");

        Destroy(this.gameObject,5);
    }
}
