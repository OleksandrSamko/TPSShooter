using UnityEngine;
using System.Collections;


public class PlayerHUD : MonoBehaviour
{

	[HideInInspector]
	public CharacterSystem character;
	public Texture2D Bg,BarHealth;
	
	void Start ()
	{
		character = gameObject.GetComponentInParent<CharacterSystem> ();
	}

	void DrawHP(int pos, string text,float percent,Texture2D bar){
		if(Bg && bar){
			GUI.DrawTexture (new Rect (80 + (pos * 180)+20, Screen.height - 55, 100, 30), Bg);		
			GUI.DrawTexture (new Rect (80 + (pos * 180)+20, Screen.height - 55, 100 * percent, 30), bar);		
			
		}
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.skin.label.fontSize = 22;
		GUI.Label (new Rect (80 + (pos * 180)+30, Screen.height - 55, 200, 30), text);		
	}
	
	void OnGUI ()
	{
		DrawHP(0,"HP "+character.HP.ToString(),((float)character.HP /(float)character.HPmax),BarHealth);
	}
}
