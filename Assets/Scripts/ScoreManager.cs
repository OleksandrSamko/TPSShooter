using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	[SerializeField]
	Text scoreText;
	[SerializeField]
	Text scoreMaxText;
	[SerializeField]
	int score=0;
	[SerializeField]
	int scoreMax=0;

	// Use this for initialization
	void Start () {
		scoreMax = score;
		scoreText.text = GetFormatString();
		scoreMaxText.text = GetFormatScoreMaxString();
	}

	string GetFormatString(){
		return string.Format("Очки: {0}",score);
	}

	string GetFormatScoreMaxString(){
		return string.Format("ТОП: {0}",scoreMax);
	}

	public int GetScore(){
		//+1 fix counter, but first fix characterSystem onDestroy
		return score+1;
	}

	// Update is called once per frame
	void Update () {
	}

	public void AddScore(int value){
		score+=value;
		scoreText.text = GetFormatString();

		if(scoreMax<score){
			scoreMax=score;	
			scoreMaxText.text = GetFormatScoreMaxString();
		}
	}

	public void RefreshScore(){
		score=0;
		scoreText.text = GetFormatString();
	}

}
