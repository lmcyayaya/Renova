﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA
{
    public class Test : MonoBehaviour
    {
        public float speed = 5;
        public float existTime=3.0f;
        float timer;
        Vector3 pos;
        public TimeManager timeManager;
        public StateManager state;
        public AnimatorHook a_hook;
        // Start is called before the first frame update
        void Start()
        {
            pos = gameObject.transform.position;
        }

        // Update is called once per frame
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
                col.GetComponent<StateManager>().Damage();
            }
        }
        private void OnTriggerEnter(Collider col)
        {
            if(col.tag=="Player")
            {
                a_hook = state.transform.GetComponentInChildren<AnimatorHook>();
                if(state.invincible && a_hook.rolling)
                {
                    timeManager.SlowmotionSet(2.0f,0.05f);
                }
            }
        }
}

}
