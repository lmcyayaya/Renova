using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    Camera cam;
    public float maximumLenght;
    public Transform target;
    public Transform[] aimStateTarget;
    public float follwSpeeed =9.0f;
    public LayerMask layerMask;
    public GameObject crossHair;
    
    private Vector3 direction;
    private Quaternion rotation;
    float counter;
    float dis;
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        
        if(cam!=null)
        {
            Ray ray = cam.ScreenPointToRay(crossHair.transform.position);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,maximumLenght,layerMask))
            {
                RotateToLookDirection(this.gameObject,hit.point);
                
            }
            else
            {
                var pos = ray.origin +(ray.direction*500);
                RotateToLookDirection(this.gameObject,pos);
            }
            Debug.DrawRay(ray.origin, ray.direction * maximumLenght, Color.yellow);
        }
        
    }
    private void FixedUpdate()
    {
        FollowTarget(Time.deltaTime);
        FollowTargerFloat();
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
        Vector3 targetPostion=Vector3.zero;
        if(Input.GetButton("L1"))
            targetPostion = Vector3.Lerp(transform.position,aimStateTarget[0].position,speed);
        else if(Input.GetButton("R2"))
        {
            if(dis>=0)
                targetPostion = Vector3.Lerp(transform.position,aimStateTarget[0].position,speed);
            else
                targetPostion = Vector3.Lerp(transform.position,aimStateTarget[1].position,speed);
            
        }
        else
            targetPostion = Vector3.Lerp(transform.position,target.position,speed);
        transform.position = targetPostion;
    }
    void FollowTargerFloat()
    {
        float c_h = Input.GetAxis("RightAxis X");
        dis +=c_h*Time.deltaTime;
        if(dis>0.5)
            dis = 0.5f;
        else if(dis<-0.5)
            dis = -0.5f;
        target.transform.localPosition = new Vector3(dis,target.transform.localPosition.y,target.transform.localPosition.z);
    }
}
