using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

	public float mFireRate = 0.5f;
	public float mFireRange = 50f;
	public float mHitForce = 100f;
	public int mLaserDamage = 100;

	//Line render that will represent the Laser
	private LineRenderer mLaserLine;
	
	//Define if laser line is showing
	private bool mLaserLineEnabled;
	
	//Time Laser Line shows onScreen
	private WaitForSeconds mLaserDuration = new WaitForSeconds(0.05f);
	
	//Time until next fire
	private float mNextFire;
	
	// Use this for initialization
	void Start () 
	{
		//Get Line Renderer
		mLaserLine = GetComponent<LineRenderer>();
			
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && Time.time > mNextFire)
		{
			Fire();
		} 		
	}

	private void Fire()
	{
		//Get ARCamera Transform
		Transform cam = Camera.main.transform;

		//Define the time of the next fire
		mNextFire = Time.time + mFireRate;

		//Set the origin of the RayCast
		Vector3 rayOrigin =  cam.position;

		/*
		Set the origin position of the Laser Line
		It will always 10 units down from the ARCamera
		We adopted this logic for simplicity
		*/
		mLaserLine.SetPosition(0, transform.up*-10f);

		//Hold hit information
		RaycastHit hit;

		//Check if the RayCast hit something
		if(Physics.Raycast(rayOrigin, cam.forward, out hit, mFireRange))
		{
			//Set the end of Laser Line to the object hit
			mLaserLine.SetPosition(1, hit.point);
		}

		else
		{
			//Set the info of the laser line to be forward the camera
			//Using the laser range
			mLaserLine.SetPosition(1, cam.forward*mFireRange);
		}

		//Show the Laser using a coroutine
		StartCoroutine(LaserFX());
	}

	//Show Laser effects
	private IEnumerator LaserFX()
	{
		mLaserLine.enabled = true;

		//Way for a specific time to remove LineRenderer
		yield return mLaserDuration;
		mLaserLine.enabled = false;
	}
}
