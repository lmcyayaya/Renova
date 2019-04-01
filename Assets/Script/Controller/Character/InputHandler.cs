using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA
{
    public class InputHandler : MonoBehaviour
    {
        public float vertical;
        public float horizontal;
        public bool o_input;
        public bool x_input;
        public bool s_input;
        public bool t_input;

        public bool r1_input;
        public bool l1_input;
        public bool r2_input;
        public bool l2_input;
        public float r2_axis;
        public float l2_axis;

        public bool leftAxis_down;
        public bool rightAxis_down;
        public float delta;
        StateManager states;
        CameraManager camManager;
        void Start()
        {
            states = GetComponent<StateManager>();
            states.Init();

            camManager = CameraManager.singleton;
            camManager.Init(states);
        }
        void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            states.FixedTick(delta);
            camManager.Tick(delta);
        }
        void Update()
        {
            delta = Time.deltaTime;
            states.Tick(delta);
        }
        void GetInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            o_input = Input.GetButton("O");
            x_input = Input.GetButton("X");
            s_input = Input.GetButton("S");
            t_input = Input.GetButtonUp("T");
            r2_axis = Input.GetAxis("R2");
            r2_input = Input.GetButton("R2");
            if(r2_axis <0)
                r2_input = true;

            l2_axis = Input.GetAxis("L2");
            l2_input = Input.GetButton("L2");
            if(l2_axis <0)
                l2_input = true;
            r1_input = Input.GetButton("R1");
            l1_input = Input.GetButton("L1");

            rightAxis_down = Input.GetButtonUp("R3");
            leftAxis_down = Input.GetButtonUp("L3");
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
            states.moveAmount = Mathf.Clamp01(m);
            
            if(r1_input)
            {
                states.run = (states.moveAmount > 0);
            }
            else
            {
                states.run = false;
            }
            //瞄準
            if(l1_input)
            {
                if(states.canMove)
                {
                    states.aim = true;
                    camManager.aim = states.aim;
                    states.lockOn = true;
                }
                else
                {
                    states.aim = false;
                    camManager.aim = states.aim;
                    states.lockOn = false;
                }
                
            }
            else
            {
                states.aim = false;
                camManager.aim = states.aim;
                states.lockOn = false;
            }
            if(Input.GetButtonDown("L1"))
            {
                camManager.lockonTarget = null;
            }
            else if(Input.GetButtonUp("L1"))
            {
                camManager.lockonTarget = null;
            }
            //



            /* if(l2_input)
            {
                states.isTwoHanded = !states.isTwoHanded;
                states.HandleTwoHanded();
            }*/
            if(rightAxis_down)
            {


                states.lockOn = !states.lockOn;
                if(states.lockonTarget ==null)
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
                    states.lockonTarget = target.GetComponent<EnemyTarget>();
                }
                else
                {
                    states.lockonTarget =null;

                }
                camManager.lockonTarget = states.lockonTarget;
                states.lockonTransform = camManager.lockonTransform;
                camManager.lockon = states.lockOn;
            }
        }
    }
}