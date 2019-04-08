using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	public float speed = 10f;
	public float life = 3;

	// Use this for initialization
	void Start () 
	{
        Destroy(gameObject, life);
	}
	
	// Update is called once per frame
	void Update () 
	{
        transform.position += transform.forward * Time.deltaTime * speed;
	}
}
