using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="BulletPattern/Burst")]
public class Burst : BulletInterface 
{
	public Transform bulletPrefab;
	public int amount;
	public int vollyAssign;
	public float shootInterlace;
	public bool aimTarget;
	[Range(-90, 0)]
	public float gatherRangeAssign;
	[Range(-90, 0)]
	public float startRange;
	private int volly;
	private float gatherRange;

	public override void InjectCoroutine()
	{
		volly = vollyAssign;
		gatherRange = startRange;
		patternCoroutine = BurstMotion();
	}

	public override void RegisterUser(Transform user)
	{
		this.bossTrans = user;
	}
	public override void DirectionTrans(Transform target)
	{
		this.dirTrans = target;
	}
	
	public IEnumerator BurstMotion()
    {
        float bulletRot = 0.0f;	//The y-axis rotation in degrees.
	    while(volly > 0)
	    {
		    for( var i = 0; i < amount; i++)
		    {
				var targetDir = dirTrans.up;
				var dir = bossTrans.TransformVector(targetDir);
				Quaternion finalRot = (aimTarget)?Quaternion.LookRotation(dir) * Quaternion.Euler(gatherRange, bulletRot, 0):Quaternion.Euler(0, bulletRot, 0);

			    Instantiate(bulletPrefab, bossTrans.position, finalRot);	//Spawn the bullet with our rotation.
			    bulletRot += 360.0f/amount;		//Increment the rotation for the next shot.
		    }
		    bulletRot = 0.0f;
			gatherRange += (gatherRangeAssign - startRange)/volly;
		    volly--;
		    yield return new WaitForSeconds(shootInterlace);
	    }
    }
}
