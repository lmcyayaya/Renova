using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    public class InputHandler : MonoBehaviour
    {
        public float vertical;
        public float horizontal;
        public bool o_input;
        public bool x_input;
        public bool s_input;
        public bool t_input;
        public bool up_input;
        public bool down_input;
        public bool left_input;
        public bool right_input;
        public bool r1_input;
        public bool l1_input;
        public bool r2_input;
        public bool l2_input;
        public bool r3_input;
        public bool l3_input;
        public bool options_input;
        public bool pausing;
        public float r2_axis;
        public float l2_axis;
        public float arrowVertical;
        public float arrowHorizontal;

        public float delta;
        public GameObject lockOnIcon;
        public GameObject[] canvas;
        private Vector3 velocity;
        private bool arrowButtonDown;
        private bool rollButtonDown;
        private bool jumpButtonDown;
        private bool lockOnButtonDown;
        private bool lookToEnemyButtonDown;
        private Animator lockOnAnim;
        public StateManager states;
        CameraManager camManager;
        void Start()
        {
            BaseData.Instance.Init(this);
            states = GetComponent<StateManager>();
            states.Init();

            camManager = CameraManager.singleton;
            camManager.Init(states);

            lockOnAnim = lockOnIcon.GetComponent<Animator>();

        }
        void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            camManager.Tick(delta);
            states.FixedTick(delta);
            
        }
        void Update()
        {
            ArrowInput();
            delta = Time.deltaTime;
            states.Tick(delta);
            // if(lockOnAnim.GetCurrentAnimatorStateInfo(0).normalizedTime>1f)
            //     lockOnIcon.SetActive(false);
            // if(lockOnIcon.activeSelf)
            //     if(camManager.lockonTransform!=null)
            //     lockOnIcon.transform.position = Camera.main.WorldToScreenPoint(camManager.lockonTransform.position+new Vector3(0,1,0));
        }
        void GetInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            o_input = Input.GetButton("O");
            s_input = Input.GetButton("S");
            t_input = Input.GetButtonUp("T");
            l1_input = Input.GetButton("L1");
            l3_input = Input.GetButton("L3");
            options_input = Input.GetButtonDown("Options");

            RollInput();
            FireInput();
            JumpInput();
            
        }
        void UpdateStates()
        {
            
            states.horizontal = horizontal;
            states.vertical = vertical;
            states.r1 = r1_input;
            states.r2 = r2_input;
            states.l1 = l1_input;
            states.l2 = l2_input;
            states.O = o_input;
            states.X = x_input;
            states.S = s_input;
            states.T = t_input;
            Vector3 v = vertical * camManager.transform.forward;
            Vector3 h = horizontal *  camManager.transform.right;
            states.moveDir = (v+h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.SmoothDamp(states.moveAmount,Mathf.Clamp01(m),ref velocity.x,0.15f);
            AimInput();
            LockOnInput();
            LookToEnemyInput();
            OptionsInput();
        }

        void ArrowInput()
        {
            arrowVertical = Input.GetAxis("ArrowVertical");
            if(arrowVertical > 0 && !arrowButtonDown)
            {
                up_input = true;
                down_input = false;
                arrowButtonDown = true;
            }
            else if(arrowVertical<0 && !arrowButtonDown)
            {
                up_input = false;
                down_input = true;
                arrowButtonDown = true;
            }
            else if(arrowVertical==0)
            {
                up_input = false;
                down_input = false;
                arrowButtonDown = false;
            }
            else
            {
                up_input = false;
                down_input = false;
            }
            arrowHorizontal = Input.GetAxis("ArrowHorizontal");
        }
        void FireInput()
        {
            r2_axis = Input.GetAxis("R2");
            r2_input = Input.GetButton("R2");
            if(r2_axis <0)
                r2_input = true;
            // if(BaseData.Instance.atkModeData.modeName=="Regular")
            // {
            //     r2_input = Input.GetButton("R2");
            //     if(r2_axis <0)
            //         r2_input = true;
            // }
            // else if(BaseData.Instance.atkModeData.modeName=="Desire")
            // {
            //     r2_input = Input.GetButton("R2");
            //     if(r2_axis <0)
            //         r2_input = true;
            // }
            // else if (BaseData.Instance.atkModeData.modeName=="Supreme")
            // {
            //     r2_input = Input.GetButton("R2");
            //     if(r2_axis <0)
            //         r2_input = true;
            // }
                
        }
        void AimInput()
        {
            //瞄準
            if(l1_input)
            {
                states.aim = true;
                states.run = false;
                camManager.aim = states.aim;
                // if(states.canMove||states.inAction)
                // {
                //     states.aim = true;
                //     states.run = false;
                //     camManager.aim = states.aim;
                // }
                // else
                // {
                //     states.aim = false;
                //     camManager.aim = states.aim;
                // }
                camManager.lockonTarget = null;
            }
            else
            {
                states.aim = false;
                camManager.aim = states.aim;
                if(camManager.lockon && camManager.lockonTransform!=null)
                {
                    camManager.lockonTarget = camManager.lockonTransform.GetComponent<EnemyTarget>();
                }
            }
        }
        void RollInput()
        {
            //l2_axis = Input.GetAxis("L2");
            //l2_input = Input.GetButton("L2");
            // if(l2_axis <0)
            //     l2_input = true;
            if(Input.GetAxis("L2") < 0 && !rollButtonDown)
            {
                rollButtonDown = true;
                l2_input = true;
            }
            else if(Input.GetAxis("L2") >=0 && rollButtonDown)
            {
                rollButtonDown = false;
                l2_input = false;
            }
            else
            {
                l2_input = false;
            }
        }
        void JumpInput()
        {
            if(Input.GetButton("X") && !jumpButtonDown)
            {
                jumpButtonDown = true;
                x_input = true;
            }
            else if(!Input.GetButton("X") && jumpButtonDown)
            {
                jumpButtonDown = false;
                x_input = false;
            }
            else
            {
                x_input = false;
            }
        }
        void LockOnInput()
        {
            if(Input.GetButton("R3") && !lockOnButtonDown)
            {
                lockOnButtonDown = true;
                r3_input = true;
            }
            else if(!Input.GetButton("R3") && lockOnButtonDown)
            {
                lockOnButtonDown = false;
                r3_input = false;
            }
            else
            {
                r3_input = false;
            }

            if(!r3_input)
                return;
            else
            {
                if(camManager.lockonTarget ==null)
                {
                    float distance = Mathf.Infinity;
                    GameObject target = null;
                    var enemys = GameObject.FindGameObjectsWithTag("Enemy");
                    for(int i= 0 ; i < enemys.Length;i++)
                    {
                        if(Vector3.Distance(transform.position,enemys[i].transform.position) < distance)
                        {
                            distance = Vector3.Distance(transform.position,enemys[i].transform.position);
                            target = enemys[i];
                        }
                    }
                    camManager.lockonTarget = target.GetComponent<EnemyTarget>();
                }
                else
                {
                    camManager.lockonTransform = null;
                    camManager.lockonTarget =null;
                }
                camManager.lockon = !camManager.lockon;
            }
        }
        void LookToEnemyInput()
        {
            if(Input.GetButton("R1") && !lookToEnemyButtonDown)
            {
                lookToEnemyButtonDown = true;
                r1_input = true;
            }
            else if(!Input.GetButton("R1") && lookToEnemyButtonDown)
            {
                lookToEnemyButtonDown = false;
                r1_input = false;
            }
            else
            {
                r1_input = false;
            }

            if(!r1_input)
                return;
            else
            {
                if(camManager.lockonTarget ==null)
                {
                    float distance = Mathf.Infinity;
                    GameObject target = null;
                    var enemys = GameObject.FindGameObjectsWithTag("Enemy");
                    for(int i= 0 ; i < enemys.Length;i++)
                    {
                        if(Vector3.Distance(transform.position,enemys[i].transform.position) < distance)
                        {
                            distance = Vector3.Distance(transform.position,enemys[i].transform.position);
                            target = enemys[i];
                        }
                    }
                    camManager.lockonTarget = target.GetComponent<EnemyTarget>();
                    camManager.lookToEnemy = true;
                    //lockOnIcon.SetActive(true);
                }
                else
                {
                    return;
                }

            }
        }
        void OptionsInput()
        {
            if(options_input)
            {
                pausing = !pausing;
                states.pausing = pausing;
                camManager.pausing = pausing;
                if(pausing)
                {
                    canvas[0].SetActive(false);
                    canvas[1].SetActive(true);
                }
                else
                {
                    canvas[0].SetActive(true);
                    canvas[1].SetActive(false);
                }
            }
        }
    }