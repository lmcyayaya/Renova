using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour 
{
	public Transform target;
	public Transform curvePoint;
	public GameObject explosion;
	[System.Serializable]
	public class SpeedStruct
	{
		public float multiScale;
		public Vector2 speedLimit;
	}
	public SpeedStruct moveSpeedData;
	public SpeedStruct rotSpeedData;
	private float reactingSpeed;
	private float rotateSpeed;
	private Collider selfCollider;
	private Vector3 velocity = Vector3.zero;
	private bool freeChase = true;
	// Use this for initialization
	void Start () 
	{
		reactingSpeed = moveSpeedData.speedLimit.x;
		rotateSpeed = rotSpeedData.speedLimit.x;
		curvePoint.parent = null;
		selfCollider  = GetComponent<Collider>();
	}
	private void OnEnable()
	{
		StartCoroutine(FreeRotateSession());		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//TODO : 飛彈需要朝向玩家做攻擊，逐漸隨著時間反應速度越來越快，最後追擊目標物

		//Step1 : 抓到面向目標的Rotation，讓飛彈可以做出Rotation.Slerp等反應
		var rotTemp = transform.rotation;
		var chaseTarget = (freeChase)? curvePoint : target ; 
		transform.LookAt(chaseTarget);
		var finalRot = transform.rotation;
		transform.rotation = rotTemp;

		transform.rotation = Quaternion.Slerp(transform.rotation, finalRot, Time.deltaTime * rotateSpeed);

		//Step2 : 讓飛彈做前進追撞
		transform.position += transform.forward * Time.deltaTime * reactingSpeed;

		//Step3 : 讓reactingSpeed逐漸加速，擬真飛彈的行為模式
		reactingSpeed = Mathf.Lerp(reactingSpeed, moveSpeedData.speedLimit.y, Time.deltaTime * moveSpeedData.multiScale);
		rotateSpeed = Mathf.Lerp(rotateSpeed, rotSpeedData.speedLimit.y, Time.deltaTime * rotSpeedData.multiScale);
	}
	private IEnumerator FreeRotateSession()
	{
		freeChase = true;

		//TODO : 一開始隨機給予砲口一個遠得要命的方向（但要限定在一定範圍內），讓飛彈可以自由衝刺，過三秒後則讓他追逐原先目標
		Vector3 chaseOffset = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), -20);
		var freeChasePos = transform.InverseTransformVector(chaseOffset) + transform.position;
		curvePoint.position = freeChasePos;
		target = curvePoint;
		yield return new WaitForSeconds(1);

		freeChase = false;
	}
	private void OnTriggerEnter(Collider other)
	{
		//產生鏡頭震動、爆炸的特效
		//CameraShake.instance.ShakeIt();
		StartCoroutine(Camera.main.GetComponent<CameraShaker>().CameraShakeOneShot(0.2f,0.05f,1.5f));
		var explore = Instantiate(explosion, transform.position, Quaternion.identity);

		//找出爆炸範圍內所有應該被影響的事物，並對其施力
		Collider[] inExploreRangeColldier= Physics.OverlapSphere (transform.position,3);
		for(int i=0 ; i<inExploreRangeColldier.Length ; i++)
		{
			var inRangeRb = inExploreRangeColldier[i].GetComponent<Rigidbody>();
			if(inRangeRb != null && inRangeRb.gameObject.tag=="Player")
				if(inRangeRb.gameObject.GetComponent<StateManager>().invincible ==false)
				{
					inRangeRb.gameObject.GetComponent<StateManager>().Damage();
					inRangeRb.AddExplosionForce(10, transform.position, 10, 3, ForceMode.Impulse);
				}
				else if(inRangeRb.gameObject.GetComponent<StateManager>().perfectDodge)
				{
					GameObject.FindGameObjectWithTag("GM").GetComponent<TimeManager>().SlowmotionSet(1f,0.05f);
				}
					
		}
		

		if(other.gameObject == target.gameObject){
			Debug.Log("Hit Target!!");
		}

		//將相關的Particle和自己的模型Destroy，包括幫助一開始的freePivot也要刪掉
		ParticleSystem parts = explore.GetComponent<ParticleSystem>();
		float totalDuration = parts.duration;
		Destroy(explore, totalDuration);
		Destroy(curvePoint.gameObject, totalDuration);
		Destroy(this.gameObject);
	}
}
