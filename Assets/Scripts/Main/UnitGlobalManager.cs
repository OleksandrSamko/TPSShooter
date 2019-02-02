using UnityEngine;
using System.Collections;

public class UnitGlobalManager : MonoBehaviour {
	
	public string GameKeyVersion = "build01";

	void Awake () {
		UnitGlobal.gameManager = (GameManager)GameObject.FindObjectOfType(typeof(GameManager));
		UnitGlobal.scoreManager = (ScoreManager)GameObject.FindObjectOfType(typeof(ScoreManager));
		UnitGlobal.GameKeyVersion = GameKeyVersion;
		Debug.Log("UnitGlobal");
	}

}

public static class UnitGlobal{

	public static GameManager gameManager;
	public static ScoreManager scoreManager;
	public static string GameKeyVersion = "";	
}