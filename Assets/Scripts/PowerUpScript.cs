//Created by Danny Gonzalez

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
	
	//Members
	public int id = 0;

	public void Hit(){
		var cam = GameObject.Find ("ARCamera");
		switch (id)
		{
			case 0://Health Pick up
				cam.GetComponent<PlayerScript> ().m_Health += 5;
				break;
			case 1://Power Bullet Pick up
				cam.GetComponent<PlayerScript> ().m_PowerBullets += 3;
				break;
			case 2://Nuke
				PlayerScript.m_Score += Nuke ();
				break;
		}
		Destroy (gameObject);
	}

	public int getId(){
		return id;
	}

	int Nuke () {
		GameObject[] Bots;
		int score = 0;
		Bots = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject bot in Bots) {
			CubeBehaviorScript cubeCtr = bot.GetComponent<CubeBehaviorScript> ();
			int health = cubeCtr.mCubeHealth;
			cubeCtr.Hit (300);
			score += health;
		}
		return score;
	}

}
