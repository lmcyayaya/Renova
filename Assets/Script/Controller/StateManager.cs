using System.Collections;
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

        [Header("Stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;

        [Header("States")]
        public bool onGround;
        public bool run;
        public bool lockOn;


        public float moveAmount;
        [HideInInspector]
        public float delta;
        public Vector3 moveDir;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rb;
        [HideInInspector]
        public LayerMask ignoreLayers;
        public void Init()
        {
           SetupAnimator();
           rb = GetComponent<Rigidbody>();
           rb.angularDrag = 999;
           rb.drag = 4;
           rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

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
    }
}
