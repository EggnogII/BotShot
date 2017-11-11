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
	private bool mLaserLine;
	
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
		
	}

	//FIRE FUNCTION GOES HERE
}
