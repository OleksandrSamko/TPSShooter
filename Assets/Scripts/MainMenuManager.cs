
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
	public string SceneStart = "scene0";
	private AudioSource audioSource;
	public AudioClip audioClip;

	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		// setup all necessary parameters.
		Application.targetFrameRate = 140;	
		// load latest scene played
		if (PlayerPrefs.GetString ("StartScene") != "") {
			SceneStart = PlayerPrefs.GetString ("StartScene");
		}
	}

	public void PlayClip(){
		audioSource.clip = audioClip;
		audioSource.Play();
	}


	public void LevelSelected (string name)
	{
		SceneStart = name;
		// save selected scene for the next round.
		PlayerPrefs.SetString ("StartScene", SceneStart);
	}
	
	public void StartPlayer ()
	{
		if (UnitGlobal.gameManager) {
			UnitGlobal.gameManager.CreateGame (SceneStart);
		}
	}
		
	public void ExitGame ()
	{
		Application.Quit ();	
	}
}
