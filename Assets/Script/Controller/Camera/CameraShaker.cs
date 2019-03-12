﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA
{
    public class CameraShaker : MonoBehaviour
    {
        public float power = 0.2f;
        public float duration = 0.05f;
        public float slowDownAmount = 1.5f;
        public Transform cam;

        Vector3 orginPos;
        float initiaDuration = 0.2f;
        void Start()
        {
            cam = Camera.main.transform;
        }
        public IEnumerator CameraShakeOneShot(float p,float dur,float sDAmount)
        {
            orginPos = cam.transform.localPosition;
            while(dur > 0)
            {
                cam.localPosition = orginPos +Random.insideUnitSphere * p;
                dur -= Time.deltaTime * sDAmount;
                yield return null;
            }
            cam.transform.localPosition = orginPos;
        }
        public void CameraKeepShake(float p)
        {
            orginPos = cam.transform.localPosition;
            cam.localPosition = orginPos +Random.insideUnitSphere * p;
        }
        public void ResetCamera()
        {
            cam.transform.localPosition = orginPos;
        }
        public IEnumerator CameraShakeTest()
        {
            initiaDuration = duration;
            orginPos = cam.transform.localPosition;
            while(duration > 0)
            {
                cam.localPosition = orginPos +Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
                yield return null;
            }
            duration = initiaDuration;
            cam.transform.localPosition = orginPos;
        }
    }
}

