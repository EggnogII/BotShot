﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviorScript : MonoBehaviour 
{

	//Cube's Max/Min scale
	public float mScaleMax = 2f;
	public float mScaleMin = 0.5f;

	//Orbit Max Speed
	public float mOrbitMaxSpeed = 30f;

	//Orbit speed
	private float mOrbitSpeed;

	//Anchor point for Cube Rotation
	private Transform mOrbitAnchor;

	//Max Cube Scale
	private Vector3 mCubeMaxScale;

	//Orbit Direction
	private Vector3 mOrbitDirection;

	//Growing Speed
	public float mGrowingSpeed = 10f;
	private bool mIsCubeScaled = false;


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
		RotateCube();

		// scale cube if needed
		if (!mIsCubeScaled){
			ScaleObj();
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