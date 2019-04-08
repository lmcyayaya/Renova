using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAssetTest : MonoBehaviour 
{
	public class Student
	{
		public string name { get; set;}
		public int score { get; set;}
	}
	// Use this for initialization
	void Start () 
	{
		List<Student> students = new List<Student>{
			new Student{name = "sekiro", score = 90},
			new Student{name = "teigo", score = 95},
			new Student{name = "error", score = 50}
		};
		var query = from stu in students
					where stu.score<=50
					select stu;
		foreach(var q in query){
			Debug.Log(string.Format("Name = \"{0}\", Score = {1}", q.name, q.score));
			Debug.LogFormat("Name = \"{0}\", Score = {1}", q.name, q.score);
		}
		// Action noTypeAction = voidNoType;
		// noTypeAction();
		// Action<string> oneTypeAction = voidOneType;
		// oneTypeAction("voidOneType");
		// Action<string, int> twoTypeAction = voidTwoType;
		// twoTypeAction("voidTwoType", 1);

		string[] str = {"charlies", "nancy", "alex", "jimmy", "selina"};
		select_S_inString(str);
	}
	Action<string[]> select_S_inString = delegate(string[] x)
	{
		var result = from p in x
					where p.Contains("s")
					select p;
		foreach(string s in result.ToList()){
			Debug.Log(s);
		}
	};
	void voidNoType()
	{
		Debug.Log("voidNoType");
	}
	void voidOneType(string _toPrint)
	{
		Debug.Log(_toPrint);
	}
	void voidTwoType(string _toPrintString, int _toPrintCount)
	{
		Debug.Log(_toPrintString + _toPrintCount.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
