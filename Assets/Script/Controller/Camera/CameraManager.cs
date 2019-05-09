using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class CameraManager : MonoBehaviour
    {
        public bool aim;
        public bool lockon;
        public float cameraDistance ;
        public float aimCameraDistance;
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
        private bool usedRightAxis;
        public bool pausing;
        public float lookSpeed;
        public Transform target;
        public Transform aimPivot;
        public Transform pausePivot;
        [HideInInspector]
        public Transform pivot;
        [HideInInspector]
        public Transform camTrans;
        StateManager states;
        [HideInInspector]
        public CameraCollision camCol;
        public GameObject crossHair;
        
        [Header("Lock")]
        public bool lookToEnemy;
        public Transform lockonTransform;
        public EnemyTarget lockonTarget;
        public float screenXMax;
        public float screenXMin;
        Vector3 lockOnPos;
        private Transform transformCache;
        public void Init(StateManager st)
        {
            states = st;
            target = st.transform;
            camTrans =Camera.main.transform;
            pivot = camTrans.parent;
            transformCache = GetComponent<Transform>();
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
            if(!aim &&!pausing)
                FollowTarget(d);
            else if(aim && !pausing)
                AimCameraMove(d);
            else
                PausingCameraMove(d);
            HandleRotations(d,v,h,targetSpeed);
        } 

        void FollowTarget(float d)
        {
            float speed = d * follwSpeeed;
            Vector3 targetPostion = Vector3.Lerp(transformCache.position,target.position,speed);
            transformCache.position = targetPostion;
            camCol.maxDistance = cameraDistance;
            //follwSpeeed = 10;
            controllerSpeed = 4;
            crossHair.SetActive(false);
        }
        void AimCameraMove(float d)
        {
            float speed = d * follwSpeeed;
            Vector3 targetPostion = Vector3.Lerp(transformCache.position,aimPivot.position,speed);
            transformCache.position = targetPostion;
            camCol.maxDistance = aimCameraDistance;
            //follwSpeeed = 10;
            controllerSpeed = 1.5f;
            crossHair.SetActive(true);
        }
        void PausingCameraMove(float d)
        {
            float speed = d * follwSpeeed *2;
            Vector3 targetPostion = Vector3.Lerp(transformCache.position,pausePivot.position,speed);
            transformCache.position = targetPostion;
            camCol.maxDistance = 4.5f;
            controllerSpeed = 4;
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

            if(!pausing)
            {
                tiltAngle -= smoothY * targetSpeed *0.8f;
                tiltAngle = Mathf.Clamp(tiltAngle,minAngle,maxAngle);
            }
            else
            {
                tiltAngle = 0;
            }
            
            pivot.localRotation = Quaternion.Euler(tiltAngle,0,0);
            
            HandleLockOnRotation(d);
            HandeleLookToEnemyRotation(d);
            lookAngle += smoothX * targetSpeed;
            transformCache.rotation = Quaternion.Euler(0,lookAngle,0);
            
        }
        void HandleLockOnRotation(float d)
        {
            if(lockonTarget != null)
            {
                 if(HandleLockDeadZone())
                     return;
                Vector3 targetDir = Camera.main.ScreenToWorldPoint(lockOnPos) - this.transformCache.position;
                targetDir.Normalize();
                if(targetDir == Vector3.zero)
                    targetDir = this.transformCache.forward;
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                targetRot.x = 0;
                targetRot.z = 0;
                transformCache.rotation = Quaternion.Slerp(transformCache.rotation,targetRot, d*lookSpeed);
                lookAngle = transformCache.rotation.eulerAngles.y;
            }
        }

        void HandeleLookToEnemyRotation(float d)
        {
            if(lookToEnemy)
            {
                Vector3 targetDir = lockonTransform.position - this.transformCache.position;
                targetDir.Normalize();

                if(targetDir == Vector3.zero)
                    targetDir = this.transformCache.forward;
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                targetRot.x = 0;
                targetRot.z = 0;
                
                if(targetRot.eulerAngles.y+5f>transformCache.rotation.eulerAngles.y &&transformCache.rotation.eulerAngles.y>targetRot.eulerAngles.y-5f)
                {
                    lockonTarget =null;
                    lockonTransform = null;
                    lockon = false;
                    lookToEnemy = false;
                    return;
                }
                
                transformCache.rotation = Quaternion.Slerp(transformCache.rotation,targetRot, d*20);
                lookAngle = transformCache.rotation.eulerAngles.y;
            }
        }
        bool HandleLockDeadZone()
        {
            screenXMax = Mathf.Clamp(screenXMax,screenXMin,16);
            screenXMin = Mathf.Clamp(screenXMin,0,screenXMax);
            lockOnPos = Camera.main.WorldToScreenPoint(lockonTransform.position);
            if(Screen.width*screenXMin/16 < lockOnPos.x && lockOnPos.x < Screen.width* screenXMax/16)
                return true;
            else
            {
                
                if(lockOnPos.x<Screen.width* screenXMin/16)
                {
                    //計算鎖定時敵人位置距離DeadZone邊境長度去讓lookSpeed做換算差值，鏡頭越靠近目標時速度就越慢，越遠則越快
                    float speedScale = Mathf.Clamp((((Screen.width* screenXMin/16)-lockOnPos.x)/500),1/12,1);
                    lookSpeed =speedScale*12;
                    lockOnPos.x = Screen.width* screenXMin/16;
                }
                else if(lockOnPos.x > Screen.width* screenXMax/16)
                {
                    
                    float speedScale = Mathf.Clamp(((lockOnPos.x-(Screen.width* screenXMax/16))/500),1/12,1);
                    lookSpeed =speedScale*12;
                    lockOnPos.x = Screen.width* screenXMax/16;
                }
                return false;
            }
        }
        public static CameraManager singleton;
        void Awake()
        {
            singleton =this;
        }
    }

