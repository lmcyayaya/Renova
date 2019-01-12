﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA
{
    public class StateManager : MonoBehaviour
    {        
        [Header("Init")]
        public GameObject activeModel;

        [Header("Inputs")]
        public float vertical;
        public float horizontal;
        public bool r1,r2,l1,l2;
        public bool twoHanded;

        [Header("Stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;

        [Header("States")]
        public bool onGround;
        public bool run;
        public bool lockOn;
        public bool inAction;
        public bool canMove;
        public bool isTwoHanded;

        public float moveAmount;
        [HideInInspector]
        public float delta;
        public Vector3 moveDir;

        [HideInInspector]
        public AnimatorHook a_hook;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rb;
        [HideInInspector]
        public LayerMask ignoreLayers;
        float _actionDelay;
        public void Init()
        {
           SetupAnimator();
           rb = GetComponent<Rigidbody>();
           rb.angularDrag = 999;
           rb.drag = 4;
           rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

           a_hook = activeModel.AddComponent<AnimatorHook>();
           a_hook.Init(this);

           gameObject.layer = 9;
           ignoreLayers = ~(1<<10);
           anim.SetBool("onGround",true);
        }
        public void SetupAnimator()
        {
            if(activeModel == null)
            {
                anim =GetComponentInChildren<Animator>();
                if(anim == null)
                {
                    Debug.Log("No Model Found");
                }
                else
                {
                    activeModel = anim.gameObject;
                }
            }
            if(anim ==null)
                anim = activeModel.GetComponent<Animator>();

            anim.applyRootMotion = false;
        }
        public void FixedTick(float d)
        {
            delta = d;

            DetectAction();

            if(inAction)
            {
                anim.applyRootMotion = true;
                _actionDelay += delta;
                if(_actionDelay>0.3f)
                {
                    inAction = false;
                    _actionDelay = 0;
                }
                else
                {
                    return;
                }
                
            }
            canMove = anim.GetBool("canMove");    

            if(!canMove)
                return;

            anim.applyRootMotion = false;

            rb.drag = (moveAmount > 0 || onGround == false) ? 0:4;

            /*  同上
            if(moveAmount > 0)
            {
                rb.drag = 0;
            }
            else
            {
                rb.drag = 4;
            }*/

            float targetSpeed = moveSpeed;
            if(run)
                targetSpeed = runSpeed;
            if(onGround)
                rb.velocity = moveDir * (targetSpeed * moveAmount) ;

            if(run)
            {
                lockOn = false;
            }

            if(!lockOn)
            {
                Vector3 targetDir = moveDir;
                targetDir.y = 0;
                if(targetDir == Vector3.zero)
                    targetDir = Vector3.forward;
                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation,tr,delta * moveAmount * rotateSpeed);
                transform.rotation =targetRotation;
            }
            

            HandleMovementAnimations();
        }

        public void DetectAction()
        {

            if(!canMove)
                return;
            if(r1 == false && r2 == false && l1 == false && l2 == false)
                return;
            string targetAnim = null;
            
            if(r1)
                targetAnim = "oh_attack_1";
            if(r2)
                targetAnim = "oh_attack_2";
            if(l2)
                targetAnim = "oh_attack_3";
            if(l1)
                targetAnim = "th_attack_1";

            if(targetAnim==null)
                return;
            canMove = false;
            inAction = true;
            anim.CrossFade(targetAnim,0.2f);
            //rb.velocity = Vector3.zero;
        }
        public void Tick(float d)
        {
            delta = d;
            onGround = OnGround();
            anim.SetBool("onGround",onGround);
        }

        void HandleMovementAnimations()
        {
            anim.SetBool("run",run);
            anim.SetFloat("vertical",moveAmount,0.4f,delta);
        }

        public bool OnGround()
        {
            bool r = false;

            Vector3 orgin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;
            float dis = toGround + 0.3f;
            RaycastHit hit;
            Debug.DrawRay(orgin,dir * dis);
            if(Physics.Raycast(orgin,dir,out hit,dis,ignoreLayers))
            {
                r = true;
                Vector3 targetPosition = hit.point;
                transform.position = targetPosition;
            }

            return r; 
        }

        public void HandleTwoHanded()
        {
            anim.SetBool("two_handed",isTwoHanded);
        }
    }
}
