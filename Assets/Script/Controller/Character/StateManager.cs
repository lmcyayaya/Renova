using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateManager : MonoBehaviour
{
    [Header("Init")]
    public GameObject activeModel;
    public GameObject model;
    public BaseData baseData;
    public ProcessedData processedData;

    [Header("Inputs")]
    public float vertical;
    public float horizontal;
    public bool r1,r2,l1,l2,O,X,S,T;

    [Header("States")]
    public bool onGround;
    public bool run;
    public bool lockOn;
    public bool inAction;
    public bool canMove;
    public bool aim;
    public bool invincible;
    public bool perfectDodge;
    public string ATKMode ;
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
    float timer;
    public void Init(BaseData data)
    {
        SetupAnimator();
        baseData = data;
        processedData = ProcessedData.Instance;
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
        ATKMode = baseData.atkModeData.modeName;
        delta = d;
        //DetectAction();
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
        HandleJump();
    }
    public void Tick(float d)
    {   
        HandleInvincibleTime(d);
        onGround = OnGround();
        anim.SetBool("onGround",onGround);
        // if(!onGround)
        //      rb.velocity +=new Vector3(0,-9.8f*d,0);
        anim.SetFloat("gravity",this.gameObject.GetComponent<Rigidbody>().velocity.y);
    }
    public void DetectAction()
    {

        if(!canMove)
            return;
        if(O == false && X == false && S == false && T == false)
            return;
        string targetAnim = null;
        /*if(X)
            targetAnim = "Jump";
            if(O)
            targetAnim = "oh_attack_1";
        if(S)
            targetAnim = "oh_attack_3";
        if(T)
            targetAnim = "th_attack_1";
        */
        if(targetAnim==null)
            return;
        canMove = false;
        inAction = true;
        anim.CrossFade(targetAnim,0.2f);
        //rb.velocity = Vector3.zero;
    }

    void HandleJump()
    {
        if(!X || !onGround)
            return;
        rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y+baseData.jumpSpeed,rb.velocity.z);
        anim.CrossFade("Jump",0.2f);
    }
    void HandleRolls()
    {
        if(!l2 )
            return;
        float v = (moveAmount>0.2f)? 1 : 0;
        float h = 0;
        if(v!=0)
        {
            if(moveDir == Vector3.zero)
                moveDir = transform.forward;
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            Quaternion rot = Quaternion.LookRotation(this.transform.forward);
            //正面翻滾
            if(rot.eulerAngles.y-45<targetRot.eulerAngles.y &&targetRot.eulerAngles.y<rot.eulerAngles.y+45)
            {
                transform.rotation = targetRot;
                a_hook.dir = AnimatorHook.Direction.Vertical;
                v = 1;
                a_hook.rm_multi = baseData.rollSpeed;
            }
            //左側翻滾
            else if(rot.eulerAngles.y-90<targetRot.eulerAngles.y &&targetRot.eulerAngles.y<rot.eulerAngles.y-45)
            {
                Quaternion Rot = Quaternion.LookRotation(this.transform.forward);
                transform.rotation = Rot;
                a_hook.dir = AnimatorHook.Direction.Horizontal;
                v = 0;
                h = -1;
                a_hook.rm_multi = baseData.rollSpeed;
            }
            //右側翻滾
            else if(rot.eulerAngles.y+45<targetRot.eulerAngles.y &&targetRot.eulerAngles.y<rot.eulerAngles.y+90)
            {
                Quaternion Rot = Quaternion.LookRotation(this.transform.forward);
                transform.rotation = Rot;
                    a_hook.dir = AnimatorHook.Direction.Horizontal;
                v = 0;
                h = 1;
                a_hook.rm_multi = -baseData.rollSpeed;
            }
            //背向翻滾
            else
            {
                Quaternion Rot = Quaternion.LookRotation(-moveDir);
                transform.rotation = Rot;
                a_hook.dir = AnimatorHook.Direction.Vertical;
                v = -1;
                a_hook.rm_multi = -baseData.rollSpeed;
            }
            a_hook.InitForRoll();
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
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation,tr,delta * moveAmount * baseData.rotateSpeed);
            transform.rotation = targetRotation;

            anim.SetBool("lockon",lockOn);

            if(!lockOn &&!aim)
                HandleMovementAnimations();
            else
                HandleLockOnAnimations(moveDir);
    }
    void HandleMovement()
    {
        if(onGround)
            rb.velocity = moveDir * (processedData.moveSpeed * moveAmount) ;
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

        Vector3 orgin = transform.position + (Vector3.up * baseData.toGround);
        Vector3 dir = -Vector3.up;
        float dis = baseData.toGround;//+ 0.3f;
        Debug.DrawRay(orgin,dir * dis,Color.green);
        
        if(Physics.Raycast(orgin,dir,out hit,dis,ignoreLayers))
        {
            r = true;
            Vector3 targetPosition = hit.point;
            transform.position = targetPosition;
        }
        return r; 
    }
    public void HandleInvincibleTime(float d)
    {
        if(!invincible)
            return;
        timer+=d;
        if(timer>baseData.perfectDodgeTime)
        {
            perfectDodge = false;
        }
        if(timer>baseData.invincibleTime)
        {
            timer = 0;
            invincible = false;
            if(!model.activeSelf)
            {
                model.SetActive(true);
            }
        }
    }
    public void Damage()
    {
        if(invincible)
            return;
        canMove = false;
        inAction = true;
        invincible = true;
        anim.CrossFade("damage_1",0.2f);
        StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.2f,0.05f,1.5f));
    }
}
