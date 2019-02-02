
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct casthit
{
	public int index;
	public float distance;
	public string name;
}

public class FPSRayActive : MonoBehaviour
{
	public string[] IgnoreTag = {"Player"};
	public string[] DestroyerTag = {"Scene"};

	public float raySize = 10.0f;

	void Start ()
	{	
	}

	public void ShootRayOnce (Vector3 origin, Vector3 direction, string id, string team)
	{
		// Normal Damage ray cast.
		RaycastHit hit;
		if (Physics.Raycast (origin, direction, out hit, 100.0f)) {
			// if hit
			if (hit.collider.gameObject != this.gameObject) {
				// create damage package.
				DamagePackage dm;
				dm.Damage = 50;
				dm.Normal = hit.normal;
				dm.Direction = direction;
				dm.Position = hit.point;
				dm.ID = id;
				dm.Team = team;
				
				// send Damage Package through OnHit function
				hit.collider.SendMessage ("OnHit", dm, SendMessageOptions.DontRequireReceiver);
				
			}
		}
	}

	public bool ShootRay (Vector3 origin, Vector3[] direction, int damage, float size, int hitmax, string id, string team)
	{
		// Multi piercing Damage Ray. e.g you can shoot through in many layer
		bool res = false;
		for (int b=0; b<direction.Length; b++) {
			int damages = damage;
			int hitcount = 0;
			int castcount = 0;
			RaycastHit[] hits;
			List<casthit> castHits = new List<casthit> ();
			float raySize = size;

			// Cast all objects.
			RaycastHit[] casterhits = Physics.RaycastAll (origin, direction [b], raySize);
			for (int i=0; i<casterhits.Length; i++) {
				if (casterhits [i].collider) {
					if (tagCheck (casterhits [i].collider.tag) && 
					casterhits [i].collider.gameObject != this.gameObject && 
					((casterhits [i].collider.transform.root && 
						casterhits [i].collider.transform.root != this.gameObject.transform.root &&
						casterhits [i].collider.transform.root.gameObject != this.gameObject) || 
						casterhits [i].collider.transform.root == null)) {
						castcount++;
						casthit casted = new casthit ();
						casted.distance = Vector3.Distance (origin, casterhits [i].point);
						casted.index = i;
						casted.name = casterhits [i].collider.name;
						castHits.Add (casted);
					}
				}
				
			}
			
			// Sort them by distance.
			hits = new RaycastHit[castcount];
			castHits.Sort ((x,y) => x.distance.CompareTo (y.distance));

			for (int i=0; i<castHits.Count; i++) {
				hits [i] = casterhits [castHits [i].index];
			}
			
			// so now you know which one is the first or last
			for (var i = 0; i < hits.Length && hitcount < hitmax; i++) {
				RaycastHit hit = hits [i];
				
				// Create Damage package
				DamagePackage dm;
				dm.Damage = damage;
				dm.Normal = hit.normal;
				dm.Direction = direction [b];
				dm.Position = hit.point;
				dm.ID = id;
				dm.Team = team;
				
				// send Damage Package through OnHit function
				hit.collider.SendMessage ("OnHit", dm, SendMessageOptions.DontRequireReceiver);
				res = true;
				
				// counting hit until max
				hitcount++;
				if (hitcount >= hitmax || tagDestroyerCheck (hit.collider.tag)) {
					break;
				}
				// damage reduced every hit
				int damageReduce = (int)((float)damages * 0.75f);
				damages = damageReduce;
			}
		}
		return res;
	}
	
	public bool ShootRayTest (Vector3 origin, Vector3[] direction, int damage, float size, int hitmax, string id, string team)
	{
		// Ray test. not for damage or interactive, just for checking.
		bool res = false;
		for (int b=0; b<direction.Length; b++) {
			int hitcount = 0;
			int castcount = 0;
			RaycastHit[] hits;
			List<casthit> castHits = new List<casthit> ();
			float raySize = size;

		
			RaycastHit[] casterhits = Physics.RaycastAll (origin, direction [b], raySize);
			for (int i=0; i<casterhits.Length; i++) {
				if (casterhits [i].collider) {
					if (tagCheck (casterhits [i].collider.tag) && 
					casterhits [i].collider.gameObject != this.gameObject && 
					((casterhits [i].collider.transform.root && 
						casterhits [i].collider.transform.root.gameObject != this.gameObject) || 
						casterhits [i].collider.transform.root == null)) {
						castcount++;
						casthit casted = new casthit ();
						casted.distance = Vector3.Distance (origin, casterhits [i].point);
						casted.index = i;
						casted.name = casterhits [i].collider.name;
						castHits.Add (casted);
					}
				}
				
			}

			hits = new RaycastHit[castcount];
			castHits.Sort ((x,y) => x.distance.CompareTo (y.distance));

			for (int i=0; i<castHits.Count; i++) {
				hits [i] = casterhits [castHits [i].index];
			}

			for (var i = 0; i < hits.Length && hitcount < hitmax; i++) {
				RaycastHit hit = hits [i];
				
				res = true;
				hitcount++;
				if (hitcount >= hitmax || tagDestroyerCheck (hit.collider.tag)) {
					break;
				}
				
			}
		}
		return res;
	}

	private bool tagDestroyerCheck (string tag)
	{
		for (int i=0; i<DestroyerTag.Length; i++) {
			if (DestroyerTag [i] == tag) {
				return true;	
			}
		}
		return false;
	}

	private bool tagCheck (string tag)
	{
		for (int i=0; i<IgnoreTag.Length; i++) {
			if (IgnoreTag [i] == tag) {
				return false;	
			}
		}
		return true;
	}
}
