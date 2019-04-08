using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="BulletPattern/Flower")]
public class Flower : BulletInterface
{
	public Transform bulletPrefab;
	public int rotPerFire;
	public int direction;
	public float floweringTime;
	public float shootInterlace;
	private float flowerTime;

	public override void InjectCoroutine()
	{
		flowerTime = floweringTime;
		patternCoroutine = FlowerMotion();
	}

	public override void RegisterUser(Transform user)
	{
		this.bossTrans = user;
	}
	public IEnumerator FlowerMotion()
    {
	    float bulletRot = 0.0f;
	    while( flowerTime > 0 )
	    {
		    for( var i = 0; i < direction; i++)
		    {
			    Instantiate(bulletPrefab, bossTrans.position, Quaternion.Euler( 0, bulletRot, 0));		//Spawn the bullet with our rotation;
			    bulletRot += 360.0f/direction;
		    } 
		    bulletRot += rotPerFire; 
		    if( bulletRot > 360)
		    {
			    bulletRot -= 360;
		    }
		    else if( bulletRot < 0 )
		    {
			     bulletRot += 360;
		    }
		    flowerTime -= shootInterlace;
		    yield return new WaitForSeconds( shootInterlace);
	    }
    }
}
