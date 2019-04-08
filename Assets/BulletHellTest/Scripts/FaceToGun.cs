using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToGun : MonoBehaviour 
{
	public Transform followTarget;
	public Transform gun;
	private void Start()
	{
		AimTarget();
	}
	
	// Update is called once per frame
	void Update () {
		AimTarget();
	}
	private void AimTarget()
	{
		transform.LookAt(gun);
		transform.position = followTarget.position;
	}
}
