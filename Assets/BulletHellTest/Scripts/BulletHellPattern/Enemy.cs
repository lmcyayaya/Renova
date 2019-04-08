using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public Transform target;
	public MissileController missile;
	public BulletInterface[] bulletInterface;
	public float speed = 2f;
	public bool isPrefabType;
	public bool isLoop;
	public int patternWaitSecond = 5;
	Vector3 direct = -Vector3.forward;
	Vector3 orgPos;
	private bool coroutineRunning;
	// Use this for initialization
	private void OnEnable()
	{
		if(isPrefabType)
			target = GameObject.FindGameObjectWithTag("GunDir").transform;
	}
	void Start () 
	{
		StartCoroutine(LaunchAndStopPattern());
	}

	void Update () 
	{
		if(!coroutineRunning && isLoop)
			StartCoroutine(LaunchAndStopPattern());
	}
	IEnumerator LaunchAndStopPattern()
	{
		int missileLaundCount = 0;
		coroutineRunning = true;

		if(!isPrefabType)
			MissileThree();

		foreach (var singlePattern in bulletInterface)
		{
			missileLaundCount ++;
			singlePattern.RegisterUser(this.transform);
			singlePattern.DirectionTrans(target);
			singlePattern.InjectCoroutine();
			StartCoroutine(singlePattern.Coroutine);


			if(!isPrefabType && missileLaundCount == bulletInterface.Length){
				MissileThree();
				missileLaundCount = 0;
			}


			yield return new WaitForSeconds(patternWaitSecond);
			StopCoroutine(singlePattern.Coroutine);
		}

		coroutineRunning = false;
		if(isLoop){
			StartCoroutine(LaunchAndStopPattern());
		}
	}

	private void MissileThree()
	{
		for(int i=0 ; i<3 ; i++){
			missile.LaunchMissile();
		}
	}
}
