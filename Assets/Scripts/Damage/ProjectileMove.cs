using UnityEngine;
using System.Collections;

public class ProjectileMove : MonoBehaviour {

	public float Speed = 100;
	void Start () {
		GameObject.Destroy(this.gameObject,1);
	}
	
	void FixedUpdate () {
		this.transform.position += this.transform.forward * Speed * Time.fixedDeltaTime;
	}
}
