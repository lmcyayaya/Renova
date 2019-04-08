using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public Animator myAnimator;
    public Renderer myRenderer;

    public Animator targetAnimator;
    public GameObject targetObject;

    public float time;
    public float intensity;
    public float pow =0.5f;
    public float timeMax = 60;
    public bool active;
    public string dir;
    Vector3 end;
	void Update ()
    {
        //if (CitadelDeep.hitPause > 0 || CitadelDeep.debugPause) { return; }
        if (time > 0) 
        { 
            time--; 
            active = true; 
            intensity = (time / timeMax) * 10 * pow; 
            UpdateRenderer();
        }
        //transform.localScale *= 1.03f; }
        else
        { 
            active = false;
            intensity = 0; 
            this.gameObject.SetActive(false);
        }
        if(dir != null)
        {
            if(dir =="forward")
            {
                transform.position = Vector3.MoveTowards(transform.position,end , Vector3.Distance(transform.position,end/0.6f)*Time.deltaTime);
            }
            else if(dir =="back")
            {
                transform.position = Vector3.MoveTowards(transform.position,end , Vector3.Distance(transform.position,end/0.6f)*Time.deltaTime);
            }
            else if(dir =="left")
            {
                transform.position = Vector3.MoveTowards(transform.position,end , Vector3.Distance(transform.position,end/0.6f)*Time.deltaTime);
            }
            else if(dir=="right")
            {
                transform.position = Vector3.MoveTowards(transform.position,end , Vector3.Distance(transform.position,end/0.6f)*Time.deltaTime);
            }
        }
        myAnimator.Play(targetAnimator.GetCurrentAnimatorStateInfo(6).shortNameHash, 6, targetAnimator.GetCurrentAnimatorStateInfo(6).normalizedTime);
        //myAnimator.Play(myCharacterControl.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //myAnimator.no

        foreach(AnimatorControllerParameter param in targetAnimator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Float)
            {
                myAnimator.SetFloat(param.name, targetAnimator.GetFloat(param.name));
            }
            if (param.type == AnimatorControllerParameterType.Int)
            {
                myAnimator.SetInteger(param.name, targetAnimator.GetInteger(param.name));
            }
        }

	}

    void UpdateRenderer()
    {
        myRenderer.material.SetFloat("_Intensity", intensity);
        myRenderer.material.SetFloat("_MKGlowPower", intensity);
    }

    public void Activate()
    {
        active = true;
        transform.position = targetObject.transform.position;
        transform.localScale = targetObject.transform.lossyScale;
        transform.rotation = targetObject.transform.rotation;

        myAnimator.Play(targetAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //myAnimator.Play(myCharacterControl.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //myAnimator.no

        foreach(AnimatorControllerParameter param in targetAnimator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Float)
            {
                myAnimator.SetFloat(param.name, targetAnimator.GetFloat(param.name));
            }
            if (param.type == AnimatorControllerParameterType.Int)
            {
                myAnimator.SetInteger(param.name, targetAnimator.GetInteger(param.name));
            }
        }

        myAnimator.speed = 0;
        time = timeMax + 1;
        Update();
    }
    public void DodgeImage()
    {
        active = true;
        transform.position = targetObject.transform.position;
        transform.localScale = targetObject.transform.lossyScale;
        transform.rotation = targetObject.transform.rotation;
        if(dir =="forward")
        {
            end = targetObject.transform.position + (targetObject.transform.forward * 2);
        }
        else if(dir =="back")
        {
            end = targetObject.transform.position + (-targetObject.transform.forward * 2);
        }
        else if(dir =="left")
        {
            end = targetObject.transform.position + (-targetObject.transform.right * 2);
        }
        else if(dir=="right")
        {
            end = targetObject.transform.position + (targetObject.transform.right * 2);
        }
        // myAnimator.Play(targetAnimator.GetCurrentAnimatorStateInfo(6).shortNameHash, 6, targetAnimator.GetCurrentAnimatorStateInfo(6).normalizedTime);
        // //myAnimator.Play(myCharacterControl.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        // //myAnimator.no

        // foreach(AnimatorControllerParameter param in targetAnimator.parameters)
        // {
        //     if (param.type == AnimatorControllerParameterType.Float)
        //     {
        //         myAnimator.SetFloat(param.name, targetAnimator.GetFloat(param.name));
        //     }
        //     if (param.type == AnimatorControllerParameterType.Int)
        //     {
        //         myAnimator.SetInteger(param.name, targetAnimator.GetInteger(param.name));
        //     }
        // }

        //myAnimator.speed = 0;
        time = timeMax + 1;
        Update();
    }

}