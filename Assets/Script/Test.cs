using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Test : MonoBehaviour
{
    public float speed = 5;
    public float existTime=3.0f;
    float timer;
    Vector3 pos;
    public TimeManager timeManager;
    public StateManager state;
    public AnimatorHook a_hook;
    public AfterImagePool AIP;
    public Animator anim;
    void Start()
    {
        pos = gameObject.transform.position;
    }
    void Update()
    {
        timer+=Time.deltaTime;
        if(speed!=0)
            transform.position += transform.forward * (speed*Time.deltaTime);
        else
            Debug.Log("No Speed");
        
        if(timer>existTime)
        {
            gameObject.transform.position = pos;
            timer = 0;
        }
            
    }
    private void OnTriggerStay(Collider col)
    {
        if(col.tag=="Player")
        {
            // col.GetComponent<StateManager>().Damage();
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag=="Player")
        {
            a_hook = state.transform.GetComponentInChildren<AnimatorHook>();
            if(state.perfectDodge && a_hook.rolling)
            {
                StartCoroutine(AIP.AddDodgeImage()) ;
                StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.3f,0.05f,1.5f));
                state.model.SetActive(false);
                timeManager.SlowmotionSet(1f,0.05f);
            }
        }
    }
}
