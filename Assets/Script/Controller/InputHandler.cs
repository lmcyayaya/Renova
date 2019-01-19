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
            camManager.Init(this.transform);
        }
        void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            states.FixedTick(Time.deltaTime);
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
            if(r2_axis != 1)
                r2_input = true;

            l2_axis = Input.GetAxis("L2");
            l2_input = Input.GetButton("L2");
            if(l2_axis != 1)
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

            Vector3 v = states.vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;
            states.moveDir = (v+h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);
            states.rollInput = x_input;

            if(o_input)
            {
                states.run = (states.moveAmount > 0);
            }
            else
            {
                states.run = false;
            }

            states.r1 = r1_input;
            states.r2 = r2_input;
            states.l1 = l1_input;
            states.l2 = l2_input;

            if(t_input)
            {
                states.isTwoHanded = !states.isTwoHanded;
                states.HandleTwoHanded();
            }
            if(rightAxis_down)
            {
                states.lockOn = !states.lockOn;
                if(states.lockonTarget ==null)
                {
                    states.lockOn = false;
                }
                camManager.lockonTarget = states.lockonTarget.transform;
                camManager.lockon = states.lockOn;
            }
        }
    }
}