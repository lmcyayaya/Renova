using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class RotateGunTest : MonoBehaviour
{
    Camera cam;
    public float maximumLenght;
    public float follwSpeeed =9.0f;

    private Ray rayLookAt;
    private Vector3 pos;
    private Vector3 direction;
    private Quaternion rotation;
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        
        if(cam!=null)
        {
            RaycastHit hit;
            var mousePos = Input.mousePosition;
            rayLookAt = cam.ScreenPointToRay(mousePos);
            //var shootDirection =(pivot.position-cam.transform.position).normalized;
            if(Physics.Raycast(rayLookAt.origin,rayLookAt.direction,out hit,maximumLenght))
            {
                RotateToLookDirection(this.gameObject,hit.point);
            }
            else
            {
                var pos = rayLookAt.GetPoint(maximumLenght);
                RotateToLookDirection(this.gameObject,pos);
            }
        }
        
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
    /* void FollowTarget(float d)
    {
        float speed = d * follwSpeeed;
        Vector3 targetPostion = Vector3.Lerp(transform.position,target.transform.position,speed);
        transform.position = targetPostion;
    }*/
}
