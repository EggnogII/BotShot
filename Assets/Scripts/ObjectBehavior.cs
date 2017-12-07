//Created by Imran Irfan
//Deprecated
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

	//Members
	public int mObjectHealth = 100; //Cube Health
	private bool mIsAlive = true; //Define if the Cube is Alive
	public float mScaleMax = 2f; //Cube's Max/Min scale
	public float mScaleMin = 0.5f;
	public float mOrbitMaxSpeed = 30f; //Orbit Max Speed
	public int damage = 100; //Damage
	private float mOrbitSpeed; //Orbit speed
	private Transform mOrbitAnchor; //Anchor point for Cube Rotation
	private Vector3 mObjectMaxScale; //Max Cube Scale
	private Vector3 mOrbitDirection; //Orbit Direction
	public float mGrowingSpeed = 10f; //Growing Speed
	private bool mIsObjectScaled = false;
	public Transform target; //USED for Moving Towards
	public float speed;

	//Object gets hit
	public bool Hit(int hitDamage)
	{
		if (mObjectHealth <= 0 && mIsAlive) 
		{
			StartCoroutine (DestroyObj ());
			return true;
		}
		return false;
	}

	//Check if you hit the player
	void CheckDistance()
	{
		float dist = Vector3.Distance (target.transform.position, transform.position);
		if (dist < 1f) 
		{
			Handheld.Vibrate ();
			var cam = GameObject.Find ("ARCamera");
			cam.GetComponent<PlayerScript>().m_Health -= 1;
			Destroy (gameObject);
		}
	}

	//Destroy Object
	private IEnumerator DestroyObj()
	{
		mIsAlive = false;

		//Make Cube Vanish
		GetComponent<Renderer>().enabled = false;

		/*
			We'll wait some time before destroying the element
			this is usefull when using some kind of effect
			like an explosion sound effect.
			in that case we could use the sound length as waiting time
		*/

		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}


	// Use this for initialization
	void Start () {
		ObjectSettings ();
	}

	//Set the initial object settings
	private void ObjectSettings()
	{
		mOrbitAnchor = Camera.main.transform;

		//Define the orbit direction
		float x = Random.Range(-1f, 1f);
		float y = Random.Range(-1f, 1f);
		float z = Random.Range(-1f, 1f);
		mOrbitDirection = new Vector3(x,y,z);

		//Defining speed
		mOrbitSpeed = Random.Range(5f, mOrbitMaxSpeed);

		//Defining Scale
		float scale = Random.Range(mScaleMin, mScaleMax);

		//Set object scale to 0, for it to grow
		transform.localScale = Vector3.zero;

	}
	
	// Update is called once per frame
	void Update () {
		CheckDistance ();
		RotateObject ();
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);

		//scale object if needed
		if (!mIsObjectScaled) 
		{
			ScaleObj ();
		}
	}

	//Makes the object rotate around an anchor point, and rotate around its own axis

	private void RotateObject()
	{
		//rotate object around camera
		transform.RotateAround(mOrbitAnchor.position, mOrbitDirection, mOrbitSpeed * Time.deltaTime);

		//rotate around its axis
		transform.Rotate(mOrbitDirection*30*Time.deltaTime);

	}

	//Scale object from 0-1
	private void ScaleObj()
	{
		//growing obj
		if (transform.localScale != mObjectMaxScale) {
			transform.localScale = Vector3.Lerp (transform.localScale, mObjectMaxScale, Time.deltaTime * mGrowingSpeed);
		}
		else 
		{
			mIsObjectScaled = true;
		}
	}
}
