using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class CameraManager : MonoBehaviour
    {
        public bool aim;
        public bool lockon;
        public float follwSpeeed =9.0f;
        public float mouseSpeed = 2.0f;
        public float controllerSpeed = 7.0f;
        float turnSmoothing = .1f;
        public float minAngle = -45;
        public float maxAngle = 45;
        float smoothX;
        float smoothY;
        float smoothXvelocity;
        float smoothYvelocity;
        public float lookAngle;
        public float tiltAngle;

        bool usedRightAxis;


        public Transform target;
        public EnemyTarget lockonTarget;
        public Transform lockonTransform;
        public Transform aimPivot;

        [HideInInspector]
        public Transform pivot;
        [HideInInspector]
        public Transform camTrans;
        StateManager states;
        [HideInInspector]
        public CameraCollision camCol;
        public GameObject crossHair;
        public void Init(StateManager st)
        {
            states = st;
            target = st.transform;
            camTrans =Camera.main.transform;
            pivot = camTrans.parent;
        }
        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float c_h = Input.GetAxis("RightAxis X");
            float c_v = Input.GetAxis("RightAxis Y");

            float targetSpeed = mouseSpeed;

            if(lockonTarget!=null)
            {
                if(lockonTransform == null)
                {
                    lockonTransform =lockonTarget.GetTarget();
                    states.lockonTransform = lockonTransform;
                }

                if(Mathf.Abs(c_h) > 0.6f)
                {
                    if(!usedRightAxis)
                    {
                        lockonTransform = lockonTarget.GetTarget((c_h > 0 ));
                        states.lockonTransform = lockonTransform;
                        usedRightAxis = true;
                    }
                }
            }

            if(usedRightAxis)
            {
                if(Mathf.Abs(c_h) < 0.6f)
                {
                    usedRightAxis = false;
                }
            }

            if(c_h!=0 || c_v!=0)
            {
                h = c_h;
                v = c_v;
                targetSpeed = controllerSpeed;
            }
            if(!aim)
                FollowTarget(d);
            else
                AimCameraMove(d);
            HandleRotations(d,v,h,targetSpeed);
        } 

        void FollowTarget(float d)
        {
            float speed = d * follwSpeeed;
            Vector3 targetPostion = Vector3.Lerp(transform.position,target.transform.position,speed);
            transform.position = targetPostion;
            camCol.maxDistance = 6;
            follwSpeeed = 4;
            controllerSpeed = 4;
            crossHair.SetActive(false);
        }
        void AimCameraMove(float d)
        {
            float speed = d * follwSpeeed;
            Vector3 targetPostion = Vector3.Lerp(transform.position,aimPivot.transform.position,speed);
            transform.position = targetPostion;
            camCol.maxDistance = 3;
            follwSpeeed = 15;
            controllerSpeed = 2.5f;
            crossHair.SetActive(true);
        }
        void HandleRotations(float d,float v,float h, float targetSpeed)
        {
            if(turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX,h,ref smoothXvelocity,turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY,v,ref smoothYvelocity,turnSmoothing);
            }
            else
            {
                smoothX = h;
                smoothY = v;
            }

            tiltAngle -= smoothY * targetSpeed *0.8f;
            tiltAngle = Mathf.Clamp(tiltAngle,minAngle,maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);
            

            if(lockon && lockonTarget != null)
            {
                Vector3 targetDir = lockonTransform.position - this.transform.position;
                targetDir.Normalize();

                if(targetDir == Vector3.zero)
                    targetDir = this.transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                targetRot.x = 0;
                targetRot.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation,targetRot,d*9);
                lookAngle = transform.eulerAngles.y;
                return;
            }

            lookAngle += smoothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0,lookAngle,0);
            
            
        }
        public static CameraManager singleton;
        void Awake()
        {
            singleton =this;
        }
    }

