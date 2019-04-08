using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	public static CameraShake instance = null;  

	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public float shakeDuration = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	
	Vector3 originalPos;
	private bool trigger = false;
	
	void Awake()
	{
		if (instance == null)
                instance = this;

		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	public void ShakeIt()
	{
		trigger = true;
		shakeDuration = .5f;
		//originalPos = camTransform.position;
	}

	void Update()
	{
		if(!trigger)
			return;

		if (shakeDuration > 0)
		{
			camTransform.localPosition = camTransform.position + Random.insideUnitSphere * shakeAmount;
			
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			trigger = false;
			shakeDuration = 0f;
			camTransform.localPosition = camTransform.position;
		}
	}
}