using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpRotChange : MonoBehaviour 
{
	public Transform influence;
	private void Update()
	{
		var dir = transform.TransformVector(Vector3.up);
		Debug.DrawLine(transform.position, transform.position + dir * 10, Color.red);
		Debug.Log(dir);
		Quaternion finalRot = Quaternion.LookRotation(dir);
		influence.rotation = finalRot;
	}
}
