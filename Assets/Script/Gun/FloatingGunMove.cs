using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingGunMove : MonoBehaviour
{
    Camera cam;
    public GameObject target;
    public float follwSpeeed ;
    private CameraManager camManager;
    private StateManager states;
    private Transform transformCache;

    void Start()
    {
        transformCache = this.GetComponent<Transform>();
        states = target.GetComponent<StateManager>();
        cam = Camera.main; 
        camManager = cam.transform.root.GetComponent<CameraManager>();  
    }
    void FixedUpdate()
    {
        FollowTarget(Time.fixedDeltaTime);
        RotateToCamera();
    }
    void FollowTarget(float d)
    {
        float speed = 0;
        if(camManager.pausing)
        {
            speed = d * follwSpeeed*5;
        }
        else if(states.aim)
        {
            speed = d * follwSpeeed*10;
        }
        else
        {
            speed = d * follwSpeeed;
        }
        Vector3 targetPostion = Vector3.Lerp(transform.position,target.transform.position,speed);
        transformCache.position = targetPostion;
    }
    void RotateToCamera()
    {
        var rotation = cam.transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        //this.gameObject.transform.localRotation =Quaternion.Lerp(this.gameObject.transform.rotation,rotation,0.1f);
        transformCache.localRotation =rotation;
    }
}
