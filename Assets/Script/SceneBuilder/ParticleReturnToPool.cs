using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReturnToPool : MonoBehaviour
{
    float t;

    // Start is called before the first frame update
    void Start()
    {
        var ps = this.gameObject.GetComponent<ParticleSystem>();
        if(ps!=null)
        {
            t = ps.main.duration;
        }
        else
        {
            var ps2 = this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
            t = ps2.main.duration;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(ObjectPool.ReturnToPool(this.gameObject,t));
    }
}
