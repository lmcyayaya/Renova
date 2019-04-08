using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DamageText : MonoBehaviour
{
    private Vector3 scale;
    public bool moveCheck;
    public Vector3 end;
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(this.transform.position-Camera.main.transform.position);
        if(moveCheck)
        {
            transform.position =Vector3.MoveTowards(this.transform.position,end,(Vector3.Distance(this.transform.position,end)/0.8f)*Time.deltaTime);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(ObjectPool.ReturnToPool(this.gameObject,0.8f));
    }
}
