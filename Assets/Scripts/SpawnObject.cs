//Created by Imran Irfan

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SpawnObject : MonoBehaviour {

	//Object to spawn
	public GameObject mObj;

	//Quantity of Objects to be spawned
	public int mTotalObjs;

	//Time to Spawn
	public float mTimeToSpawn = 0.8f;

	//Container to hold objects
	private GameObject[] mObjects;

	// Flag if the position has been set
	private bool mPositionSet;

	//Which kind of enemy?
	public int enemyType;

	// Use this for initialization
	void Start () {
		//Initializing spawn loop
		StartCoroutine(SpawnLoop());

		mObjects = new GameObject[mTotalObjs];
	}

	// Loop Spawning cube elements
	private IEnumerator SpawnLoop()
	{
		//Defining the Spawning Position
		StartCoroutine(ChangePosition());

		yield return new WaitForSeconds (0.2f);

		//Spawn the elements
		int i=0;
		while (i <= (mTotalObjs - 1)) 
		{
			if (Camera.main.GetComponentInParent<PlayerScript> ().isPaused == false) {
				mObjects [i] = SpawnElement ();
				i++;
			}
				yield return new WaitForSeconds (Random.Range (mTimeToSpawn, mTimeToSpawn * 3));
			
		}
	}

	private void Green(GameObject obj)
	{
		obj.GetComponent<Renderer> ().material.color = Color.green;
	}

	private void Blue(GameObject obj)
	{
		obj.GetComponent<Renderer> ().material.color = Color.blue;
	}

	private void Red(GameObject obj)
	{
		obj.GetComponent<Renderer> ().material.color = Color.red;
	}

	private void Pink(GameObject obj)
	{
		obj.GetComponent<Renderer> ().material.color = new Color (1f,0.5f,0.5f,1f);
	}

	private void Purple(GameObject obj)
	{
		obj.GetComponent<Renderer> ().material.color = new Color (0.5f,0f,0.5f,1f);
	}

	private void Yellow(GameObject obj)
	{
		obj.GetComponent<Renderer> ().material.color = new Color (1f, 1f, 0f, 1f);
	}

	//Spawn an Object
	private GameObject SpawnElement()
	{
		// Spawn the element on a random position, use a sphere for reference
		GameObject obj = Instantiate (mObj, (Random.insideUnitSphere * 4) + transform.position, transform.rotation)
			as GameObject;

		if (enemyType == 1) {
			Green (obj);
		} else if (enemyType == 2) {
			Blue (obj);
		} else if (enemyType == 3) {
			Red (obj);
		} else if (enemyType == 4) {//Health
			Pink (obj);
		} else if (enemyType == 5) {//PowerShot
			Purple (obj);
		} else if (enemyType == 6) {
			Yellow (obj);
		}

		//Define a random scale for the object
		float scale = Random.Range(0.5f, 2f);

		//Change the object scale
		obj.transform.localScale = new Vector3(scale, scale, scale);
		return obj;
	}

	//Define the position of the object according to the ARCamera position
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
		yield return new WaitForSeconds (0.2f);
		//Define new spawn position only once

		if (!mPositionSet) 
		{
			//Change the position only if Vuforia is active
			if (VuforiaBehaviour.Instance.enabled)
				SetPosition ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
