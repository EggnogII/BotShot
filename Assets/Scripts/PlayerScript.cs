//Created by Imran Irfan and Danny Gonzalez

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	//Player Members
	public Text m_scoreText;
	public Text m_healthText;
	public Text m_powerText;
	public int m_Health = 3;
	public static int m_Score = 0;
	public int m_PowerBullets = 0;
	public int sceneIndex;
	public bool isPaused = false;


	//Laser Members
	public float mFireRate = 0.5f;
	public float mFireRange = 50f;
	public float mHitForce = 100f;
	public int mLaserDamage = 100;
	public AudioClip clip;
	public AudioClip hitConfirmSound;
	public AudioClip hitPowerupSound;
	public AudioClip nukeSound;
	private LineRenderer mLaserLine;
	private bool mLaserLineEnabled;
	private WaitForSeconds mLaserDuration = new WaitForSeconds (0.05f);
	public float mNextFire;

	//Pause Button members
	public Button pauseButton;

	void CheckHealth()
	{
		if (m_Health < 1) 
		{
			//GameOVer
			SceneManager.LoadScene(sceneIndex);
		}
	
	}

	public void Pause(){
		//If Paused
		if (isPaused == true) {
			pauseButton.GetComponentInChildren<Text> ().text = "Pause";
			isPaused = false;
		} 
		//Not Paused
		else {
			pauseButton.GetComponentInChildren<Text> ().text = "Resume";
			isPaused = true;
		}
	}

	// Use this for initialization
	void Start () {
		m_Score = 0;
		m_Health = 3;
		//Get the Line Renderer
		mLaserLine = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckHealth ();
		m_scoreText.text = "Score: " + m_Score.ToString ();
		m_healthText.text = "Health: " + m_Health.ToString ();
		m_powerText.text = "Power Bullets: " + m_PowerBullets.ToString();
		if (Input.GetButton ("Fire1") && Time.time > mNextFire && (isPaused==false)) 
		{
			Fire ();
		}
		
	}

	private void Fire(){

		int health;

		//Get the ARCamera Transform
		Transform cam = Camera.main.transform;

		//Define the time of the next fire
		mNextFire = Time.time + mFireRate;

		//Set the origin of the RayCast
		Vector3 rayOrigin =  cam.position;

		//GetComponent<AudioSource> ();
		AudioSource.PlayClipAtPoint(clip, rayOrigin);
		/*
		Set the origin position of the Laser Line
		It will always 10 units down from the ARCamera
		We adopted this logic for simplicity
		*/
		mLaserLine.SetPosition(0, transform.up*-10f);

		//Hold hit information
		RaycastHit hit; 

		//Check if Raycast hit something
		if (Physics.Raycast (rayOrigin, cam.forward, out hit, mFireRange)) {
			//Set the end of the Laser Line to the object hit
			mLaserLine.SetPosition (1, hit.point);

			//Get Power Up thing
			PowerUpScript powscript = hit.collider.GetComponent<PowerUpScript> ();
			if (powscript != null) 
			{
				//Play Audio in powerup script?
				if (powscript.getId() == 2){
					AudioSource.PlayClipAtPoint(nukeSound, rayOrigin);
				}
				else{
					AudioSource.PlayClipAtPoint(hitPowerupSound, rayOrigin);
				}
				powscript.Hit ();
			}

			//Get CubeBehavior script to apply damage to target
			CubeBehaviorScript cubeCtr = hit.collider.GetComponent<CubeBehaviorScript> ();
			if (cubeCtr != null) {
				if (hit.rigidbody != null) {
					//Play hitConfirm sound
					AudioSource.PlayClipAtPoint(hitConfirmSound, rayOrigin);
					//Apply force to target
					hit.rigidbody.AddForce (-hit.normal * mHitForce);

					//Apply damage to target
					health = cubeCtr.mCubeHealth;
					if (m_PowerBullets > 0) {
						m_PowerBullets = m_PowerBullets - 1;
						cubeCtr.Hit (mLaserDamage*3);
					} else {
						cubeCtr.Hit (mLaserDamage);
					}
					//Tally score
					m_Score += health;

				}
			}
		} 

		else 
		{
			//Set info of Laser Line to be forward the camera
			mLaserLine.SetPosition(1, cam.forward*mFireRange);
		}

		StartCoroutine (LaserFX ());
	}

	//Show special effect for laser
	private IEnumerator LaserFX()
	{
		mLaserLine.enabled = true;

		//Wait for a specific time to remove LineRenderer
		yield return mLaserDuration;
		mLaserLine.enabled = false;
	}

}
