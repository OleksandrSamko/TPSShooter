
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemySpawner : MonoBehaviour
{

	public GameObject[] Objects;
	public float TimeSpawn = 3;
	public int MaxObject = 10;
	public string PlayerTag = "Player";
	public bool PlayerEnter = true;
	private float timetemp = 0;
	private int indexSpawn;
	private List<GameObject> spawnList = new List<GameObject> ();
	public bool OnActive;
	public bool spawnOnStart;

	void Start ()
	{
		indexSpawn = Random.Range (0, Objects.Length);
		timetemp = Time.time;
		Spawn();
	}

	void Spawn(){
		GameObject obj = null;
		Vector3 spawnPoint = transform.position + new Vector3 (Random.Range (-(int)(this.transform.localScale.x / 2.0f), (int)(this.transform.localScale.x / 2.0f)),0, Random.Range ((int)(-this.transform.localScale.z / 2.0f), (int)(this.transform.localScale.z / 2.0f)));
			obj = (GameObject)GameObject.Instantiate (Objects [indexSpawn], spawnPoint, Quaternion.identity);
		if (obj)
			spawnList.Add (obj);
		indexSpawn = Random.Range (0, Objects.Length);

	}

	void Update ()
	{
		OnActive = false;
		if (PlayerEnter) {
			// check if player is enter this area
			GameObject[] playersaround = GameObject.FindGameObjectsWithTag (PlayerTag);
			for (int p=0; p<playersaround.Length; p++) {
				if (Vector3.Distance (this.transform.position, playersaround [p].transform.position) < this.transform.localScale.x) {
					OnActive = true;
				}
			}
		} else {
			OnActive = true;
		}

		if (!OnActive)
			return;
		
		ObjectExistCheck ();
		if (Objects [indexSpawn] == null)
			return;
		
		// spawn if ObjectsNumber is less than Max object.
		if (ObjectsNumber < MaxObject && Time.time > timetemp + TimeSpawn) {
			timetemp = Time.time;
			Spawn();
		}
	}
	
	private int ObjectsNumber;

	void ObjectExistCheck ()
	{
		// checking a number of all objects. that's was spawn with this spawner
		ObjectsNumber = 0;
		foreach (var obj in spawnList) {
			if (obj != null)
				ObjectsNumber++;
		}
	}
	
	void OnDrawGizmos ()
	{
		#if UNITY_EDITOR
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, 0.3f);
		Gizmos.DrawWireCube (transform.position, this.transform.localScale);
		Handles.Label(transform.position, "Enemy Spawner");
		#endif
	}
	
}
