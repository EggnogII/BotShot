//Created by Imran Irfan

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviorScript : MonoBehaviour 
{
	//Members
	public int mCubeHealth = 100; //Cube Health
	private bool mIsAlive = true; //Define if the Cube is Alive
	public float mScaleMax = 2f; //Cube's Max/Min scale
	public float mScaleMin = 0.5f;
	public float mOrbitMaxSpeed = 30f; //Orbit Max Speed
	public int damage = 100; //Damage
	private float mOrbitSpeed; //Orbit speed
	private Transform mOrbitAnchor; //Anchor point for Cube Rotation
	private Vector3 mCubeMaxScale; //Max Cube Scale
	private Vector3 mOrbitDirection; //Orbit Direction
	public float mGrowingSpeed = 10f; //Growing Speed
	private bool mIsCubeScaled = false;
	public Transform target; //USED for Moving Towards
	public float speed;

	//Cube gets hit
	//Return false when cube destroyed
	public bool Hit(int hitDamage)
	{
		
			mCubeHealth -= hitDamage;
			if (mCubeHealth <= 0 && mIsAlive) {
				StartCoroutine (DestroyCube ());
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

	//Destroy Cube
	private IEnumerator DestroyCube()
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
		CubeSettings();
	}

	//Set the initial cube settings
	private void CubeSettings()
	{

		mOrbitAnchor = Camera.main.transform;

		// defining the orbit direction
		float x = Random.Range(-1f, 1f);
		float y = Random.Range(-1f, 1f);
		float z = Random.Range(-1f, 1f);
		mOrbitDirection = new Vector3(x,y,z);

		//defining speed
		mOrbitSpeed = Random.Range(5f, mOrbitMaxSpeed);

		//defining scale
		float scale = Random.Range(mScaleMin, mScaleMax);
		mCubeMaxScale = new Vector3(scale, scale, scale);

		//set cube scale to 0, for it to grow
		transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

		if (Camera.main.GetComponentInParent<PlayerScript> ().isPaused == false) {

			CheckDistance ();
			RotateCube ();
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);

			// scale cube if needed
			if (!mIsCubeScaled) {
				ScaleObj ();
			}
		}
	}
		
	/*
		Makes the cube rotate around a anchor point
		and rotate arount its own axis
	*/
	private void RotateCube()
	{
		//rotate cube around camera
		transform.RotateAround(
			mOrbitAnchor.position, mOrbitDirection, mOrbitSpeed * Time.deltaTime);

		// rotating around its axis
		transform.Rotate( mOrbitDirection * 30 * Time.deltaTime);
	}

	//Scale object from 0-1
	private void ScaleObj()
	{
		//growing obj
		if (transform.localScale != mCubeMaxScale)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, mCubeMaxScale, Time.deltaTime*mGrowingSpeed);

		}
		else
		{
			mIsCubeScaled = true;
		}
	}
}
