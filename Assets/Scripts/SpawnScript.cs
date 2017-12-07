//Created by Imran Irfan
//Deprecated
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SpawnScript : MonoBehaviour 
{

	// Cube element to spawn
	public GameObject mCubeObj;

	// Qtd of Cubes to be spawned
	public int mTotalCubes = 10;

	// Time to spawn
	public float mTimeToSpawn = 0.8f;

	// Hold all the cubes
	private GameObject[] mCubes;

	// Define if position was set
	private bool mPositionSet;

	// Use this for initialization
	void Start () 
	{
		//Initializing spawning loop
		StartCoroutine( SpawnLoop() );
		//StartCoroutine(ChangePosition());
	
		mCubes = new GameObject[ mTotalCubes ]; 
	}

	// Loop Spawning cube elements
	private IEnumerator SpawnLoop()
	{
		//Defining the Spawning Position
		StartCoroutine(ChangePosition());

		yield return new WaitForSeconds(0.2f);

		// Spawn the elements
		int i=0;
		while (i <= (mTotalCubes-1) )
		{
			mCubes[i]= SpawnElement();
			i++;
			yield return new WaitForSeconds(Random.Range(mTimeToSpawn, mTimeToSpawn*3));
		}
	}

	// Spawn a cube
	private GameObject SpawnElement()
	{
		// spawn the element on a random position, inside a imaginary sphere
		GameObject cube = Instantiate(mCubeObj, (Random.insideUnitSphere*4) + transform.position, transform.rotation) as GameObject;

		// define a random scale for the cube
		float scale = Random.Range(0.5f, 2f);

		// change the cube scale
		cube.transform.localScale = new Vector3(scale, scale, scale);
		return cube;
	}

	/*
		Define the position of object according to ARCamera position
	*/
	private bool SetPosition()
	{
		//Get Camera Position
		Transform cam = Camera.main.transform;

		//Set position 10 units forward from the camera position
		transform.position =cam.forward*10;
		return true;
	}

	private IEnumerator ChangePosition()
	{
		yield return new WaitForSeconds(0.2f);
		//Define new spawn position only once
		if (!mPositionSet)
		{
			//Change the position only if Vuforia is active
			if (VuforiaBehaviour.Instance.enabled)
				SetPosition();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
