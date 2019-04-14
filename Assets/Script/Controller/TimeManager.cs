using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class TimeManager : MonoBehaviour
    {
        float slowdownFactor = 0.05f;
        float slowdownLength = 2f;
        bool beSlowmotion = false;
        void Update()
        {
            
            Time.timeScale +=(1f/slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale,0f,1f);
            Time.fixedDeltaTime = Time.timeScale * .02f;
            if(Time.timeScale ==1)
            {
                beSlowmotion = false;
            }
        }
        public void SlowmotionSet(float length,float scale )
        {
            if(beSlowmotion)
                return;
            beSlowmotion = true;
            slowdownLength = length;
            Time.timeScale = scale;
        }
    }

