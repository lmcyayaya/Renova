using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour 
{
	[Range(1, 10)]
	public float speed = 5;
	private Rigidbody rb;
	private bool grounding = true;
	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody>();	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if(!grounding)
			return;


		var x_speed = Input.GetAxis("Horizontal");
		var z_speed = Input.GetAxis("Vertical");

		if(x_speed != 0 || z_speed != 0){
			Vector3 dir = new Vector3(x_speed, 0, z_speed).normalized;
			// Quaternion rot = Quaternion.LookRotation(dir);

			// transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * speed);
			// transform.rotation = rot;

			rb.velocity = dir * speed;
		}

		if(Input.GetKeyDown(KeyCode.E)){
			rb.AddExplosionForce(5, transform.position + new Vector3(1, 0, 1), 10, 3, ForceMode.Impulse);
		}
	}
	private void Update()
	{
		if(Physics.Raycast(transform.position, Vector3.down, 1)){
			grounding = true;
		}else{
			grounding = false;
		}
			
	}
}
