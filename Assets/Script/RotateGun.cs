﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class RotateGun : MonoBehaviour
{
    Camera cam;
    StateManager states;
    public float maximumLenght;
    public Transform target;
    public Transform pivot;
    public float follwSpeeed =9.0f;

    private Ray rayLookAt;
    private Vector3 pos;
    private Vector3 direction;
    private Quaternion rotation;
    void Start()
    {
        states = target.parent.GetComponent<StateManager>();
        cam = Camera.main;
    }
    void Update()
    {
        if(cam!=null)
        {
            RaycastHit hit;
            var mousePos = Input.mousePosition;
            rayLookAt = cam.ScreenPointToRay(mousePos);
            if(Physics.Raycast(cam.transform.position,pivot.position,out hit,maximumLenght))
            {
                RotateToLookDirection(this.gameObject,hit.point);
            }
            else
            {
                var pos = (pivot.position - cam.transform.position) * maximumLenght;
                RotateToLookDirection(this.gameObject,pos);
            }
        }
    }
    private void FixedUpdate()
    {
        FollowTarget(Time.deltaTime);
    }

    void RotateToLookDirection(GameObject obj,Vector3 destination)
    {
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation,rotation,1);
    }
    public Quaternion GetRotation()
    {
        return rotation;
    }
    void FollowTarget(float d)
    {
        float speed = d * follwSpeeed;
        Vector3 targetPostion = Vector3.Lerp(transform.position,target.transform.position,speed);
        transform.position = targetPostion;
    }
}
