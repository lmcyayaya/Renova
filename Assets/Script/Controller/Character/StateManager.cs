using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StateManager : MonoBehaviour
{
    public Vector3 velocity;
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

    //[Header("States")]
    public bool onGround
    {
        get
        {
            Vector3 orgin = transform.position + (Vector3.up * BaseData.Instance.toGround);
            Vector3 dir = -Vector3.up;
            float dis = BaseData.Instance.toGround+0.3f;
            if(Physics.Raycast(orgin,dir,out hit,dis,ignoreLayers))
            {
                transform.position = hit.point;
                rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
                jump2 = false;
                return true;
            }
            else
                return false;
        }
    }
    [Header("States")]
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
    public Vector3 onSlopeMoveDir;
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
    [HideInInspector]
    private RaycastHit hit;
    private float _actionDelay;
    private float timer;
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

        gameObject.layer = 0;
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
        ATKMode = BaseData.Instance.atkModeData.modeName;
        delta = d;
        HandleGravity(d);
        HandleLockRotationAnimations(moveDir);
        HandleRun();
        if(jump2 && Input.GetButton("X") && rb.velocity.y<0)
        {
            gravityScale = 1;
        }else
        {
            gravityScale = 5;
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
        a_hook.CloseRoll();
        anim.applyRootMotion = false;
        rb.drag = (moveAmount > 0 || onGround == false) ? 0:4;
        HandleMovement();
        HandleLookAndBodyAngle();
        HandleRoll();
        //HandleRolls();
        HandleJump();
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

    void HandleRun()
    {
        if(!run)
            return;
        if(moveAmount==0)
            run = false;
    }
    void HandleJump()
    {
        if(!X)
            return;
        if(!jump2)
        {
            if(!onGround)
                jump2 =true; 
            rb.velocity = new Vector3(rb.velocity.x,BaseData.Instance.jumpSpeed,rb.velocity.z);
            anim.CrossFade("Jump",0.2f);
        }
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
                a_hook.rm_multi = BaseData.Instance.rollSpeed;
            }
            //左側翻滾
            else if(rot.eulerAngles.y-90<targetRot.eulerAngles.y &&targetRot.eulerAngles.y<rot.eulerAngles.y-45)
            {
                Quaternion Rot = Quaternion.LookRotation(this.transform.forward);
                transform.rotation = Rot;
                a_hook.dir = AnimatorHook.Direction.Horizontal;
                v = 0;
                h = -1;
                a_hook.rm_multi = BaseData.Instance.rollSpeed;
            }
            //右側翻滾
            else if(rot.eulerAngles.y+45<targetRot.eulerAngles.y &&targetRot.eulerAngles.y<rot.eulerAngles.y+90)
            {
                Quaternion Rot = Quaternion.LookRotation(this.transform.forward);
                transform.rotation = Rot;
                    a_hook.dir = AnimatorHook.Direction.Horizontal;
                v = 0;
                h = 1;
                a_hook.rm_multi = -BaseData.Instance.rollSpeed;
            }
            //背向翻滾
            else
            {
                Quaternion Rot = Quaternion.LookRotation(-moveDir);
                transform.rotation = Rot;
                a_hook.dir = AnimatorHook.Direction.Vertical;
                v = -1;
                a_hook.rm_multi = -BaseData.Instance.rollSpeed;
            }
            a_hook.InitForRoll();
        }
        else
        {
            a_hook.rm_multi = 1.3f;
        }
        anim.SetFloat("horizontal",h);
        anim.SetFloat("vertical",v);

        //model.SetActive(false);
        model.transform.localScale=new Vector3(0.2f,0.2f,0.2f);
        var effect = Instantiate(effectPrefab,model.transform.position,Quaternion.identity);
        effect.transform.position = model.transform.position;
        Destroy(effect,effect.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);
        StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.5f,0.1f,2f));
        perfectDodge = true;
        canMove = false;
        inAction = true;
        invincible = true;
        dodgeCount+=1;
        dodgeCountText.text = "Dodge:"+dodgeCount.ToString();
        anim.CrossFade("Rolls",0.2f);
    }




    void HandleRoll()
    {
        if(!l2 )
            return;
        float v = (moveAmount>0.3)?1 :0;
        float h = horizontal;
        a_hook.dir = AnimatorHook.Direction.Vertical;
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
        dodgeCount+=1;
        dodgeCountText.text = "Dodge:"+dodgeCount.ToString();
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
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation,tr,delta * moveAmount * BaseData.Instance.rotateSpeed);
        transform.rotation = targetRotation;

        anim.SetBool("lockon",lockOn);
        if(!lockOn &&!aim)
            HandleMovementAnimations();
        else
            HandleLockRotationAnimations(moveDir);
    }
    void HandleMovement()
    {
        rb.velocity = moveDir * (ProcessedData.Instance.moveSpeed * moveAmount) + new Vector3(0,rb.velocity.y,0) ;
    }
    void HandleGravity(float d)
    {
        if(a_hook.rolling)
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

    public void HandleInvincibleTime(float d)
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
                //model.SetActive(true);
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

    void HandleMovementAnimations()
    {
        anim.SetBool("run",run);
        anim.SetFloat("vertical",moveAmount,0.4f,delta);
    }
    void HandleLockRotationAnimations(Vector3 moveDir)
    {
        Vector3 relativeDir = transform.InverseTransformDirection(moveDir);
        float h = relativeDir.x;
        float v = relativeDir.z;
        anim.SetFloat("horizontal",h);
        anim.SetFloat("vertical",v);
    }

}
