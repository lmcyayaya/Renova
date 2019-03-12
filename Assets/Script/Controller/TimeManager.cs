using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA
{
    public class TimeManager : MonoBehaviour
    {
        public float slowdownFactor = 0.6f;
        public float slowdownLength = 1f;
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
        public void Slowmotion()
        {
            if(beSlowmotion)
                return;
            beSlowmotion = true;
            Time.timeScale = slowdownFactor;
        }
    }
}

