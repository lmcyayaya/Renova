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
        public float walkSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;
        public float rollSpeed = 1;
        public float invincibleTime = 0.3f;
        public float perfectDodgeTime = 0.16f;
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
        public bool perfectDodge;

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
        [HideInInspector]
        RaycastHit hit;
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
            Debug.Log(perfectDodge);
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
            a_hook.CloseRoll();
            
        
            anim.applyRootMotion = false;

            rb.drag = (moveAmount > 0 || onGround == false) ? 0:4;
            HandleMovement();
            HandleLookAndBodyAngle();
            HandleRolls();
        }
        public void Tick(float d)
        {   
            HandleInvincibleTime(d);
            onGround = OnGround();
            anim.SetBool("onGround",onGround);
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

        void HandleRolls()
        {
            if(!l2 || !onGround)
                return;
            float v = (moveAmount>0.2f)? 1 : 0;
            float h = 0;
             if(v!=0)
            {
                if(moveDir == Vector3.zero)
                    moveDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                Quaternion rot = Quaternion.LookRotation(this.transform.forward);
                if(rot.eulerAngles.y-45<targetRot.eulerAngles.y &&targetRot.eulerAngles.y<rot.eulerAngles.y+45)
                {
                    transform.rotation = targetRot;
                    a_hook.rm_multi = rollSpeed;
                }
                else
                {
                    Quaternion Rot = Quaternion.LookRotation(-moveDir);
                    transform.rotation = Rot;
                    v=-1;
                    a_hook.rm_multi = -rollSpeed;
                }
                //transform.rotation = targetRot;
                a_hook.InitForRoll();
                //a_hook.rm_multi = rollSpeed;
            }
            else
            {
                a_hook.rm_multi = 1.3f;
            }
            anim.SetFloat("horizontal",h);
            anim.SetFloat("vertical",v);

            perfectDodge = true;
            canMove = false;
            inAction = true;
            invincible = true;
            invincibleTime = 0.3f;
            anim.CrossFade("Rolls",0.2f);
            
            
        }

        void HandleLookAndBodyAngle()
        {
            Vector3 targetDir;
            targetDir = (aim) ? ((Camera.main.transform.position+Camera.main.transform.forward*8)-this.transform.position)
                : (lockOn==false) ? moveDir 
                    : (lockonTransform !=null) ? lockonTransform.transform.position - this.transform.position
                        : moveDir;
                targetDir.y = 0;
                if(targetDir == Vector3.zero)
                    targetDir = Vector3.forward;
                Quaternion tr = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation,tr,delta * moveAmount * rotateSpeed);
                transform.rotation = targetRotation;

                anim.SetBool("lockon",lockOn);

                if(!lockOn &&!aim)
                    HandleMovementAnimations();
                else
                    HandleLockOnAnimations(moveDir);
        }
        void HandleMovement()
        {
            float moveSpeed = walkSpeed;
            if(run)
                moveSpeed = runSpeed;
            if(onGround)
                rb.velocity = moveDir * (moveSpeed * moveAmount) ;
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
            if(v<0.6 && v > -0.6)
            {  
                v = 0;
            }
            else
            {
                h =0;
            }
            anim.SetFloat("horizontal",h,0.2f,delta);
            anim.SetFloat("vertical",v,0.2f,delta);
        }

        public bool OnGround()
        {
            bool r = false;

            Vector3 orgin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;
            float dis = toGround + 0.3f;
            Debug.DrawRay(orgin,dir * dis,Color.green);
            
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
        public void HandleInvincibleTime(float d)
        {
            if(!invincible)
                return;
            timer+=d;
            if(timer>perfectDodgeTime)
            {
                perfectDodge = false;
            }
            if(timer>invincibleTime)
            {
                timer = 0;
                invincible = false;
            }
        }
        public void Damage()
        {
            if(invincible)
                return;
            canMove = false;
            inAction = true;
            invincible = true;
            invincibleTime = 0.2f;
            anim.CrossFade("damage_1",0.2f);
            StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.2f,0.05f,1.5f));
        }
    }
    
}
