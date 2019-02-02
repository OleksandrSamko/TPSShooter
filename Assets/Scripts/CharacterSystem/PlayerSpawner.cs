
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;


public class PlayerSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject player;

	void Start ()
	{
	
	}

	void OnDrawGizmos ()
	{
		#if UNITY_EDITOR
		Gizmos.color = Color.green;
		Gizmos.DrawSphere (transform.position, 0.1f);
		Gizmos.DrawWireCube (transform.position, this.transform.localScale);
		Handles.Label (transform.position, "Player Spawner");
		#endif
	}

	void CheckPlayerExist ()
	{
		
	}

	public GameObject Spawn ()
	{
		Vector3 spawnPosition = this.transform.position + new Vector3 (Random.Range (-(int)(this.transform.localScale.x / 2.0f), (int)(this.transform.localScale.x / 2.0f)), 0, Random.Range (-(int)(this.transform.localScale.z / 2.0f), (int)(this.transform.localScale.z / 2.0f)));
		GameObject goPlayer;
		Debug.Log ("Spawn " + player.name);
		goPlayer = (GameObject)GameObject.Instantiate (player, spawnPosition, Quaternion.identity);
	
		return goPlayer;
	}

}
