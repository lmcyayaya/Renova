using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletDamageControl : MonoBehaviour
{
    // Start is called before the first frame update
    public TimeManager timeManager;
    public StateManager state;
    public AnimatorHook a_hook;
    private void Start()
    {
        timeManager = GameObject.FindGameObjectWithTag("GM").GetComponent<TimeManager>();
    }
    private void OnTriggerStay(Collider col)
        {
            if(col.tag=="Player")
            {
                state = col.GetComponent<StateManager>();
                state.Damage();
            }
        }
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag=="Player")
        {
            state = col.GetComponent<StateManager>();
            a_hook = state.transform.GetComponentInChildren<AnimatorHook>();
            if(state.perfectDodge && a_hook.rolling)
            {
                timeManager.SlowmotionSet(1f,0.05f);
            }
        }
    }
}
