using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName="BulletPattern/Blast")]
public class Blast : BulletInterface
{
	public Transform bulletPrefab;
	public int amount;
	public int vollyAssign;
	public float spreadAngle;
	public float shootInterlace;
	private int volly;

	public override void InjectCoroutine()
	{
		volly = vollyAssign;
		patternCoroutine = BlastMoiton();
	}

	public override void RegisterUser(Transform user)
	{
		this.bossTrans = user;
	}
	public override void DirectionTrans(Transform target)
	{
		this.dirTrans = target;
	}
	public IEnumerator BlastMoiton()
	{
		float bulletRot = bossTrans.eulerAngles.y;	//The y-axis rotation in degrees.
	    if ( amount <= 1 )
	    {
		    // Just fire straight.
		    Instantiate(bulletPrefab, bossTrans.position, Quaternion.Euler(0, bulletRot, 0)); 
	    }
	    else
	    {
		    while(volly > 0)
		    {
			    bulletRot = bulletRot - (spreadAngle/2);		//Offset the bullet rotation so it will start on one side of the z-axis and end on the other.
			    for( var i = 0; i < amount; i++ )
			    {
				    Instantiate(bulletPrefab, bossTrans.position, Quaternion.Euler(0, bulletRot, 0)); // Spawn the bullet with our rotation.
				    bulletRot += spreadAngle/(amount-1);	//Increment the rotation for the next shot.
				    if(shootInterlace > 0)
				    {
					    yield return new WaitForSeconds( shootInterlace );	//Wait time between shots.
				    }
			    }
			    bulletRot = bossTrans.eulerAngles.y; // Reset the default angle.
			    volly--;
		    }
	    }
	}
}