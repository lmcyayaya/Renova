using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour 
{
	public Transform attackTarget;
	public Transform missilePrefab;
	public List<Transform> muzzles;
	public bool isVolley;
	public float timeMargin;
	private bool isFire;
	private void Update()
	{
		
	}
	public void LaunchMissile()
	{
		if(isFire)StopAllCoroutines();
		StartCoroutine(Fire());
	}
	public IEnumerator Fire()
	{
		isFire = true;
		var margin = (isVolley) ? 0 : timeMargin;
		for(int i=0 ; i<muzzles.Count ; i++){
			var missile = Instantiate(missilePrefab, muzzles[i].transform.position, muzzles[i].transform.rotation);
			var data = missile.GetComponent<Missile>();
			data.target = attackTarget;
			yield return new WaitForSeconds(timeMargin);
		}
		isFire = false;
	}
}
