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
        AnimationCurve rollCurve;
        public Direction dir;
        public enum Direction
        {
            Vertical,Horizontal
        }
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

        public void CloseRoll()
        {
            if(!rolling)
                return;
            roll_t = 0;
            rm_multi = 0;
            rolling = false;
        }

        void OnAnimatorMove() 
        {
            
            if(states.canMove)
                return;
            states.rb.drag = 0;

            if(rm_multi ==0)
                rm_multi =1.3f;
            if(!rolling)
            {
                Vector3 delta = anim.deltaPosition;
                delta.y = 0;
                Vector3 v = (delta * rm_multi) / states.delta;
                states.rb.velocity = v + states.rb.velocity.y*Vector3.up;
            }
            else
            {
                roll_t +=states.delta/0.5f;
                if(roll_t >1)
                    roll_t = 1;
                //float zValue = rollCurve.Evaluate(roll_t);
                if(states.moveDir.normalized!=Vector3.zero)
                {
                    Vector3 relative = states.moveDir.normalized;
                    Vector3 v2 = (relative * rm_multi);
                    states.rb.velocity = v2 + states.rb.velocity.y*Vector3.up;
                }
                else
                {
                    Vector3 relative = Vector3.forward.normalized;
                    Vector3 v2 = (relative * rm_multi);
                    states.rb.velocity = v2 + states.rb.velocity.y*Vector3.up;
                }
                
            }
            
        }
    }

