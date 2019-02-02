using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour {
	[SerializeField]
	GameObject menu;
	[SerializeField]
	Text scoreResultText;

	[SerializeField]
	PlayerSpawner playerSpawner;
	// Use this for initialization
	void Start () {
		scoreResultText.text = GetFormatStringResultScore();
	}

	void Awake(){
		if(!menu){
			menu = this.gameObject;
		}
		menu.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(menu.activeSelf){
				HideMenu();
			}else{
				ShowMenu();
			}
		}
	}

	public void ShowMenu(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		scoreResultText.text = GetFormatStringResultScore();
		menu.SetActive(true);

	}

	string GetFormatStringResultScore(){
		return string.Format("Ваш результат: {0}",UnitGlobal.scoreManager.GetScore());
	}

	public void HideMenu(){
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		menu.SetActive(false);
	}

	public void Exit(){
		UnitGlobal.gameManager.QuitGame();
	}

	public void RequestSpawnPlayer(){
		UnitGlobal.scoreManager.RefreshScore();
		playerSpawner.Spawn();
	}

}
