using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInterface : ScriptableObject
{
	[HideInInspector]
	public Transform bossTrans;
	public Transform dirTrans;
	protected IEnumerator patternCoroutine;
	public IEnumerator Coroutine{get {return patternCoroutine;}}
	public virtual void InjectCoroutine()
	{
		Debug.Log("No Coroutine Inject");
	}

	public virtual void RegisterUser(Transform user)
	{
		Debug.Log("No Override Registing");
	}
	public virtual void DirectionTrans(Transform target)
	{
		Debug.Log("No Override DirectionTrans");
	}
}
