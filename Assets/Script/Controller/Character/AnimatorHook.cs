using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class AnimatorHook : MonoBehaviour
    {
        Animator anim;
        StateManager states;
        public float rm_multi;
        public bool rolling;
        float roll_t;
        public bool runBreak;
        AnimationCurve rollCurve;
        public void Init(StateManager st)
        {
            states = st;
            anim = st.anim;
            rollCurve = st.roll_curve;
            
        }

        public void InitForRoll()
        {
            rolling = true;
            roll_t = 0;
        }
        public void InitForRunBreak()
        {
            runBreak = true;
        }


        public void CloseRoll()
        {
            if(!rolling)
                return;
            roll_t = 0;
            rm_multi = 0;
            rolling = false;
        }
        public void CloseRunBreak()
        {
            if(!runBreak)
                return;
            rm_multi = 0;
            runBreak = false;
        }

        void OnAnimatorMove() 
        {
            
            if(states.canMove)
                return;
            states.rb.drag = 0;

            if(rm_multi ==0)
                rm_multi =1.3f;
            
            if(!rolling&&!runBreak)
            {
                Vector3 delta = anim.deltaPosition;
                delta.y = 0;
                Vector3 v = (delta * rm_multi) / states.delta;
                states.rb.velocity =states.rb.velocity.y*Vector3.up + v;
            }
            else if(rolling)
            {
                roll_t +=states.delta/0.5f;
                if(roll_t >1)
                    roll_t = 1;
                //float Value = rollCurve.Evaluate(roll_t);
                if(states.moveDir!=Vector3.zero)
                {
                    Vector3 relative = states.moveDir.normalized;
                    Vector3 v2 = (relative * rm_multi);
                    states.rb.velocity = v2 + states.rb.velocity.y*Vector3.up;
                }
                else
                {
                    Vector3 relative = transform.forward.normalized;
                    Vector3 v2 = (relative * rm_multi);
                    states.rb.velocity = v2 + states.rb.velocity.y*Vector3.up;
                }
            }
            else if(runBreak)
            {
                transform.parent.rotation = anim.rootRotation;
                Vector3 delta = anim.deltaPosition;
                delta.y = 0;
                Vector3 v = (delta * 1) / states.delta;
                states.rb.velocity = v + states.rb.velocity.y*Vector3.up;
            }
        }

    }

