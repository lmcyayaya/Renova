using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour 
{
	public Transform target;
	public float smoothSpeed = 1;
	private Vector3 offSet, posToPivot;
	// Use this for initialization
	void Start () 
	{
		offSet = transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		posToPivot = target.position + offSet;
		transform.position = Vector3.MoveTowards(transform.position, posToPivot, Time.deltaTime * smoothSpeed);
	}
}
