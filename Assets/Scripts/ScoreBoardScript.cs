//Created by Danny Gonzalez
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class ScoreBoardScript : MonoBehaviour {

	//Members
	public Text m_scoreText;
	public Text m_highscoresText;
	public InputField m_inputField;
	public int sceneIndex;

	public List<Score> scores = new List<Score> ();
	string path = "";

	//Class required to 
	public class Score
	{
		public int playerScore = 0;
		public string playerName = "NO_NAME";

		public Score(string line)
		{
			string[] words = line.Split(' ');
			int.TryParse(words[0], out playerScore);
			if (words.Length == 2)
			{
				playerName = words[1];
			}
		}
	}

	//Creates the file if it does not exist already
	void CreateScoresFile()
	{
		if (File.Exists (path)) 
		{
			Debug.Log (path + " already exists.");
			return;
		}
		StreamWriter sr = System.IO.File.CreateText (path);
		sr.Close ();
	}

	public void WriteScore()
	{
		StreamWriter writer = new StreamWriter (path, true);
		writer.WriteLine (PlayerScript.m_Score.ToString () + " " + m_inputField.text);
		writer.Close ();
		SceneManager.LoadScene (sceneIndex);
	}

	public void ReadScores()
	{
		StreamReader reader = new StreamReader (path);
		//Parse the file
		string line;
		while ((line = reader.ReadLine ()) != null) 
		{
			Score s = new Score (line);
			scores.Add (s);
		}
		reader.Close ();

		//Sort the scores in ascending order
		scores.Sort (delegate (Score score1, Score score2) 
		{
			return (score1.playerScore.CompareTo (score2.playerScore));
		});
		scores.Reverse (); //Put in descending order

		//Display top 5 scores
		int count = 0;
		foreach (Score score in scores) 
		{
			m_highscoresText.text += (score.playerScore.ToString () + "-" + score.playerName + "\n");
			count++;
			if (count == 5) break;
		}
	}

	// Use this for initialization
	void Start () {
		path = Application.persistentDataPath + "/Scores.txt";
		CreateScoresFile ();
		m_scoreText.text = "SCORE: " + PlayerScript.m_Score;
		ReadScores ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
