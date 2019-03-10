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
        public bool r1,r2,l1,l2,O,X,S,T;
        public bool twoHanded;

        [Header("Stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;
        public float rollSpeed = 1;
        public float dodgeTime = 0.2f;
        float timer;

        [Header("States")]
        public bool onGround;
        public bool run;
        public bool lockOn;
        public bool inAction;
        public bool canMove;
        public bool isTwoHanded;
        public bool aim;
        public bool invincible;

        [Header("Other")]
        public EnemyTarget lockonTarget;
        public Transform lockonTransform;
        public AnimationCurve roll_curve;

        public float moveAmount;
        [HideInInspector]
        public float delta;
        //[HideInInspector]
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
            if(invincible)
            {
                HandleDodgeTime(d);
            }
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

            //a_hook.rm_multi = 1;
            a_hook.CloseRoll();    
            HandleRolls();

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
                Debug.DrawLine(transform.position,transform.position+moveDir*5,Color.black);

            if(run)
            {
                lockOn = false;
            }
            if(!aim)
            {

                Vector3 targetDir = (lockOn==false) ? moveDir 
                : (lockonTransform !=null) ? lockonTransform.transform.position - this.transform.position
                    : moveDir;
                targetDir.y = 0;
                if(targetDir == Vector3.zero)
                    targetDir = Vector3.forward;
                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation,tr,delta * moveAmount * rotateSpeed);
                transform.rotation = targetRotation;

                anim.SetBool("lockon",lockOn);

                if(!lockOn)
                    HandleMovementAnimations();
                else
                    HandleLockOnAnimations(moveDir);
            }
            else
            {
                Vector3 targetDir =((Camera.main.transform.position+Camera.main.transform.forward*8)-this.transform.position);
                targetDir.y = 0;
                if(targetDir == Vector3.zero)
                    targetDir = Vector3.forward;
                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation,tr,delta * moveAmount * rotateSpeed);
                transform.rotation = targetRotation;

                anim.SetBool("lockon",lockOn);

                if(!lockOn)
                    HandleMovementAnimations();
                else
                    HandleLockOnAnimations(moveDir);
            }
            
        }

        public void DetectAction()
        {

            if(!canMove)
                return;
            if(O == false && X == false && S == false && T == false)
                return;
            string targetAnim = null;
            
            if(O)
                targetAnim = "oh_attack_1";
            if(X)
                targetAnim = "oh_attack_2";
            if(S)
                targetAnim = "oh_attack_3";
            if(T)
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

        void HandleRolls()
        {
            if(!l2 || !onGround)
                return;
            float v = vertical;
            float h = horizontal;

            v = (moveAmount>0.3f)? 1 : 0;
            h = 0;
            /* if(!lockOn)
            {
                v = (moveAmount>0.3f)? 1 : 0;
                h = 0;
            }
            else
            {
                if(Mathf.Abs(v) < 0.3f)
                    v = 0;
                if(Mathf.Abs(h) < 0.3f)
                    h = 0;
            }*/
            if(v!=0)
            {
                if(moveDir == Vector3.zero)
                    moveDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = targetRot;
                anim.SetFloat("horizontal",h);
                a_hook.InitForRoll();
                a_hook.rm_multi = rollSpeed; 
            }
            else
            {
                a_hook.rm_multi = 1.3f;
            }

            anim.SetFloat("vertical",v);

            canMove = false;
            inAction = true;
            invincible = true;
            anim.CrossFade("Rolls",0.2f);
            
        }

        void HandleMovementAnimations()
        {
            anim.SetBool("run",run);
            anim.SetFloat("vertical",moveAmount,0.4f,delta);
        }

        void HandleLockOnAnimations(Vector3 moveDir)
        {
            Vector3 relativeDir = transform.InverseTransformDirection(moveDir);
            float h = relativeDir.x;
            float v = relativeDir.z;

            anim.SetFloat("horizontal",h,0.2f,delta);
            anim.SetFloat("vertical",v,0.2f,delta);
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
        public void HandleDodgeTime(float d)
        {
            timer+=d;
            if(timer>dodgeTime)
            {
                timer = 0;
                invincible = false;
            }
        }
        public void Damage()
        {
            if(invincible )
                return;
            canMove = false;
            inAction = true;
            invincible = true;
            anim.CrossFade("damage_1",0.2f);
        }

    }
    
}
