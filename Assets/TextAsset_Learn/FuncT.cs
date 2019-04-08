using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FuncT : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Func<int> func = xxx;
		Debug.Log(func());
	}
	private int CallStringLength(string str)
	{
		return str.Length;
	}
	private int xxx()
	{
		return 10;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
