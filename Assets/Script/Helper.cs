using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SA{
    public class Helper : MonoBehaviour
    {
        [Range(-1,1)]
        public float vertical;
        [Range(-1,1)]
        public float horizontal;
        public string[] oh_attacks;
        public string[] th_attacks;
        public bool AnimPlay;
        public bool twoHanded;
        public bool enableRM;
        public bool useItem;
        public bool interacting;
        public bool lockon;
        Animator anim; 
        void Start()
        {
            anim= GetComponent<Animator>();
        }
        void Update()
        {
            
            enableRM = !anim.GetBool("canMove");
            anim.applyRootMotion = enableRM;
            interacting = anim.GetBool("interacting");

            if(!lockon)
            {
                horizontal = 0;
                vertical = Mathf.Clamp01(vertical);
            }

            anim.SetBool("lockon",lockon);



            if(enableRM)
                return;
            
            if(useItem)
            {
                anim.Play("use_item");
                useItem = false;
            }

            if(interacting)
            {
                AnimPlay = false;
                vertical = Mathf.Clamp(vertical,0,0.5f);
            }

            anim.SetBool("two_handed",twoHanded);

            if(AnimPlay)
            {
                string targetAnim;
                if(!twoHanded)
                {
                    int r = Random.Range(0,oh_attacks.Length);
                    targetAnim = oh_attacks[r];
                }
                else
                {
                    int r = Random.Range(0,th_attacks.Length);
                    targetAnim = th_attacks[r];
                }
                vertical = 0;
                anim.CrossFade(targetAnim,0.2f);
                AnimPlay = false;
            }


            anim.SetFloat("vertical",vertical);
            anim.SetFloat("horizontal",horizontal);
        }
    }

}
