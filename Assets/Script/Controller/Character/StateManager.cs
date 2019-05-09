using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StateManager : MonoBehaviour
{
    public Vector3 velocity;
    public float bodyAngle;
    public float bodyDir;
    [Header("Init")]
    public GameObject activeModel;
    public GameObject model;
    public SpawnProjectiles spawnProjectiles;
    public Text dodgeCountText;
    private int dodgeCount = 0;

    [Header("Inputs")]
    public float vertical;
    public float horizontal;
    public bool r1,r2,l1,l2,O,X,S,T,jump2;
    public bool onGround
    {
        get
        {
            Vector3 orgin = transform.position + (Vector3.up * BaseData.Instance.toGround);
            Vector3 dir = -Vector3.up;
            float dis = BaseData.Instance.toGround+0.05f;//+0.3f;
            if(Physics.Raycast(orgin,dir,out hit,dis,ignoreLayers))
            {
                transform.position = hit.point;
                //rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
                jump2 = false;
                return true;
            }
            else
                return false;
        }
    }
    [Header("States")]
    public bool pausing;
    public bool run;
    public bool inAction;
    public bool canMove;
    public bool aim;
    public bool invincible;
    public bool perfectDodge;
    public string ATKMode;
    [Header("Other")]
    public float moveAmount;
    public AnimationCurve roll_curve;
    [HideInInspector]
    public float delta;
    //[HideInInspector]
    public Vector3 moveDir;
    public float gravityScale;
    public GameObject effectPrefab;
    [HideInInspector]
    public AnimatorHook a_hook;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rb;
    public LayerMask ignoreLayers;
    private RaycastHit hit;
    private Vector3 movedirSave;
    private float _actionDelay;
    private float _actionDelayMaxTime;
    private float timer;
    private float rotateSpeed = 1;
    public void Init()
    {
        SetupAnimator();
        
        spawnProjectiles.Init(this);

        rb = GetComponent<Rigidbody>();
        rb.angularDrag = 999;
        rb.drag = 4;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        a_hook = activeModel.AddComponent<AnimatorHook>();
        a_hook.Init(this);

        //ignoreLayers = ~(1<<10);
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
        ATKMode = BaseData.Instance.atkModeData.modeName;
        delta = d;

        HandleGravity(d);
        HandleRun();
        

        if(pausing)
        {
            ResetStates();
            ResetAnimation();
            return;
        }
            
        if(inAction)
        {
            anim.applyRootMotion = true;
            //anim.SetFloat("angle",0);
            anim.SetFloat("direction",0);
            _actionDelay += delta;
            HandleLockRotationAnimations(moveDir);
            if(_actionDelay>_actionDelayMaxTime)
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
        a_hook.CloseRunBreak();

        anim.applyRootMotion = false;
        rb.drag = (moveAmount > 0 || onGround == false) ? 0:4;
        HandleMovement();
        bodyAngle = ReturnCurrentBodyAndMoveAngle();
        HandleLookAndBodyAngle();
        HandleRoll();
        HandleJump();
        HandleGliding();
        
        HandleRunBreak();
        
    }
    public void Tick(float d)
    {   
        velocity = rb.velocity;
        HandleInvincibleTime(d);
        anim.SetBool("onGround",onGround);
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
    float ReturnCurrentBodyAndMoveAngle()
    {
        
        Vector3 axisSign = Vector3.Cross(moveDir,transform.forward);
        float angleOut = Vector3.Angle(transform.forward, moveDir)* (axisSign.y >= 0 ? -1f : 1f);
        bodyDir = angleOut/180 ;
        if(moveDir==Vector3.zero ||aim)         
            return 0;
        else
            return angleOut;
    }
    void HandleRun()
    {
        if(!run)
            return;
        if(moveAmount<=0.5f)
        {
            run = false;
            rotateSpeed = 1;
        }
            
    }
    void HandleJump()
    {
        if(!X||bodyAngle>135||bodyAngle<-135)
            return;
        if(!jump2)
        {
            if(!onGround)
            {

                jump2 =true;
                anim.CrossFade("Frontsault",0.2f);
                rb.velocity = new Vector3(rb.velocity.x,BaseData.Instance.jumpSpeed,rb.velocity.z);
                return;
            }
                
            rb.velocity = new Vector3(rb.velocity.x,BaseData.Instance.jumpSpeed,rb.velocity.z);
            anim.CrossFade("Jump",0.2f);
        }
    }
    void HandleGliding()
    {
        gravityScale = 5;
        if(!jump2)
            return;
        if(Input.GetButton("X") && rb.velocity.y<0)
            gravityScale = 1;
    }
    void HandleRoll()
    {
        if(!l2 )
            return;
        float v = (moveAmount>0.3)?1 :0;
        float h = horizontal;
        a_hook.rm_multi = BaseData.Instance.rollSpeed;
        a_hook.InitForRoll();
        if(v==0)
            v=1;
        anim.SetFloat("horizontal",h);
        anim.SetFloat("vertical",v);

        //model.SetActive(false);
        //model.transform.localScale=new Vector3(0.2f,0.2f,0.2f);
        var effect = Instantiate(effectPrefab,model.transform.position,Quaternion.identity);
        effect.transform.position = model.transform.position;
        Destroy(effect,effect.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);
        StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.5f,0.1f,2f));
        perfectDodge = true;
        canMove = false;
        inAction = true;
        invincible = true;
        run = true;
        rotateSpeed = 4;
        _actionDelayMaxTime = 0.3f;
        dodgeCount+=1;
        dodgeCountText.text = "Dodge:"+dodgeCount.ToString();
        anim.CrossFade("Rolls",0.2f);
    }
    void HandleLookAndBodyAngle()
    {
        Vector3 targetDir;
        targetDir = (aim) ?(Camera.main.transform.forward)
        // ((Camera.main.transform.position+Camera.main.transform.forward*8)-this.transform.position)
                    : moveDir;
        
        targetDir.y = 0;
        if(targetDir == Vector3.zero)
            targetDir = transform.forward;
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation,tr,delta * BaseData.Instance.rotateSpeed*rotateSpeed);
        transform.rotation = targetRotation;

        anim.SetBool("lockon",aim);

        if(!aim)
            HandleMovementAnimations();
        else
            HandleLockRotationAnimations(moveDir);
    }
    void HandleMovement()
    {
        if(moveDir!=Vector3.zero)
        {
            movedirSave = moveDir;
            rb.velocity = moveDir * (ProcessedData.Instance.moveSpeed * moveAmount) + new Vector3(0,rb.velocity.y,0);
        }
        else
            rb.velocity = movedirSave * (ProcessedData.Instance.moveSpeed * moveAmount) + new Vector3(0,rb.velocity.y,0);
    }
    void HandleGravity(float d)
    {
        if(a_hook.rolling||onGround)
        {
            rb.useGravity = false;
            rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
        }
        else
        {
            rb.useGravity = true;
            rb.velocity = new Vector3(rb.velocity.x,rb.velocity.y-(9.8f*d*gravityScale),rb.velocity.z);
        }
            
    }
    void HandleRunBreak()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion Normal") &&onGround)
        {
            if(bodyAngle>135 && moveAmount>=0.6)
            {
                a_hook.InitForRunBreak();
                inAction = true;
                canMove = false;
                anim.SetFloat("angle",bodyAngle);
                anim.CrossFade("JogBreakRight",0.3f);
                _actionDelayMaxTime = 0.6f;
            }
            else if(bodyAngle<-135 && moveAmount>=0.6)
            {
                a_hook.InitForRunBreak();
                inAction = true;
                canMove = false;
                anim.SetFloat("angle",bodyAngle);
                anim.CrossFade("JogBreakLeft",0.3f);
                _actionDelayMaxTime =0.6f;
            }
        }
        // else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Run") && aim==false)
        // {
        //     if(bodyAngle>135 && moveAmount>=0.5)
        //     {
        //         a_hook.InitForRunBreak();
        //         inAction = true;
        //         canMove = false;
        //         anim.SetFloat("angle",bodyAngle);
        //         anim.CrossFade("RunBreakRight",0.1f);
        //         _actionDelayMaxTime = 0.6f;
        //     }
        //     else if(bodyAngle<-135 && moveAmount>=0.5)
        //     {
        //         a_hook.InitForRunBreak();
        //         inAction = true;
        //         canMove = false;
        //         anim.SetFloat("angle",bodyAngle);
        //         anim.CrossFade("RunBreakLeft",0.1f);
        //         _actionDelayMaxTime =0.6f;
        //     }
        // }
            

    }
    void HandleInvincibleTime(float d)
    {
        if(!invincible)
            return;
        timer+=d;
        model.transform.localScale = Vector3.MoveTowards(model.transform.localScale,new Vector3(1,1,1),0.1f);
        if(timer>BaseData.Instance.perfectDodgeTime)
        {
            perfectDodge = false;
        }
        if(timer>BaseData.Instance.invincibleTime)
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
        _actionDelayMaxTime = 0.2f;
        invincible = true;
        anim.CrossFade("damage_1",0.2f);
        StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.2f,0.05f,1.5f));
    }

    void HandleMovementAnimations()
    {
        anim.SetBool("run",run);
        anim.SetFloat("moveAmount",moveAmount,0.1f,delta);
        anim.SetFloat("direction",bodyDir,0.05f,delta);
    }
    void HandleLockRotationAnimations(Vector3 moveDir)
    {
        Vector3 relativeDir = transform.InverseTransformDirection(moveDir).normalized *moveAmount;
        float h = relativeDir.x;
        float v = relativeDir.z;
        anim.SetFloat("horizontal",h,0.1f,delta);
        anim.SetFloat("vertical",v,0.1f,delta);
        anim.SetBool("run",run);
    }
    void ResetAnimation()
    {
        anim.applyRootMotion = false;
        anim.SetBool("run",false);
        anim.SetFloat("moveAmount",0);
        anim.SetFloat("direction",0);
        anim.SetFloat("horizontal",0);
        anim.SetFloat("vertical",0);
        anim.SetFloat("angle",0);
        anim.CrossFade("Locomotion Normal",0.1f);
    }
    void ResetStates()
    {
        run = false;
        rb.velocity = Vector3.zero;
    }
    private void OnTriggerStay(Collider col)
    {
        if(col.tag == "Bullet")
        {
            TimeManager.Instance.SlowmotionSet(2,0.1f);
        }
    }
}
