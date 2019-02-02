using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour {
	[SerializeField]
	private int healthValue;
	[SerializeField]
	private float lifeTime;
	// Use this for initialization
	void Start () {
		Destroy(this,lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		//get player
		PlayerCharacter player = other.GetComponent<PlayerCharacter>();
		if(player){
			//add hp	
			player.AddHP(healthValue);
			Destroy(this);
		}
		//else nothing
	}
}
