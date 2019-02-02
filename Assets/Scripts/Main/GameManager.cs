
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	public string UserName = "";
	public string Team = "";
	public string UserID = "";

	[HideInInspector]
	public string PlayingLevel;
	[HideInInspector]
	public string CurrentLevel;

	void Awake ()
	{
		DontDestroyOnLoad (this.gameObject);
	}
	
	void Start ()
	{
		PlayerPrefs.SetString ("landingpage", Application.loadedLevelName);
		UserName = PlayerPrefs.GetString ("user_name");
	}
	
	void Update ()
	{		
		CurrentLevel = Application.loadedLevelName;
	}
		
	public void CreateGame (string startlevel)
	{
		// Create game.
		PlayingLevel = startlevel;
		StartGame(PlayingLevel);
	}

	public void StartGame (string level)
	{
		Application.LoadLevel (level);
	}
	
	public void RestartGame ()
	{
		StartGame(PlayingLevel);
	}
	
	public void QuitGame ()
	{
		if (Application.loadedLevelName != PlayerPrefs.GetString ("landingpage")) {
			Application.LoadLevel (PlayerPrefs.GetString ("landingpage"));
			GameObject.Destroy (this.gameObject);
		}
	}
}