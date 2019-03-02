using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingGunMove : MonoBehaviour
{
    Camera cam;
    public GameObject target;
    public float follwSpeeed =9.0f;

    void Start()
    {
        cam = Camera.main;   
    }
    void FixedUpdate()
    {
        
        FollowTarget(Time.fixedDeltaTime);
        RotateToCamera();
    }
    void FollowTarget(float d)
    {
        float speed = d * follwSpeeed;
        Vector3 targetPostion = Vector3.Lerp(transform.position,target.transform.position,speed);
        transform.position = targetPostion;
    }
    void RotateToCamera()
    {
        var rotation = cam.transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        this.gameObject.transform.localRotation =Quaternion.Lerp(this.gameObject.transform.rotation,rotation,0.1f);
    }
}
