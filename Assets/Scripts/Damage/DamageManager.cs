using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DamageManager : MonoBehaviour
{
	public int HP = 100;
	public int HPmax = 100;
	public int Armor = 0;
	public int Armormax = 100;
	public GameObject DeadReplacement;
	public float DeadReplaceLifeTime = 180;
	public bool isAlive = true;
	public AudioClip[] SoundPain;
	public AudioSource Audiosource;
	[HideInInspector]
	public string Team = "";
	[HideInInspector]
	public string ID = "";
	[HideInInspector]
	public string LastHitByID = "";

	private bool initialized = false;
	private Vector3 directionHit;
	
	void Start ()
	{
		Audiosource = this.GetComponent<AudioSource> ();
		initialized = false;
	}

	void Update ()
	{
		DamageUpdate ();
	}

	public void DamageUpdate ()
	{
		if (isAlive) {
			if (HP > HPmax)
				HP = HPmax;
			
			if (Armor > Armormax)
				Armor = Armormax;
			
			if (HP <= 0) {
					isAlive = false;
					OnDead ();
			}
			//UpdateData ();
		}
	}

	public void AddHP(int hpAdd){
		HP+=hpAdd;
	}

	public void ApplyDamage (int damage, Vector3 direction, string attackerID, string team)
	{
			directionHit = direction;
			LastHitByID = attackerID;
			if (Team != team || team == "") {	
				HP -= damage;
			}	

		if (Audiosource && SoundPain.Length > 0) {
			Audiosource.PlayOneShot (SoundPain [Random.Range (0, SoundPain.Length)]);	
		}
	}
	
	public void DirectDamage (DamagePackage pack)
	{
		ApplyDamage ((int)((float)pack.Damage), pack.Direction, pack.ID, pack.Team);
	}
	
	void OnDead ()
	{
		this.gameObject.SendMessage("OnThisThingDead",SendMessageOptions.DontRequireReceiver);
		GameObject.Destroy (this.gameObject);
	}

	void OnDestroy ()
	{
		if (DeadReplacement && !Application.isLoadingLevel) {
			GameObject deadbody = (GameObject)GameObject.Instantiate (DeadReplacement, this.transform.position, Quaternion.identity);
			CopyTransformsRecurse (this.transform, deadbody);
			DeadReplaceLifeTime = 3;
			GameObject.Destroy (deadbody, DeadReplaceLifeTime);
		}
	}
	
	public void CopyTransformsRecurse (Transform src, GameObject dst)
	{
		dst.transform.position = src.position;
		dst.transform.rotation = src.rotation;
		if (dst.GetComponent<Rigidbody>())
			dst.GetComponent<Rigidbody>().AddForce (directionHit, ForceMode.VelocityChange);
		foreach (Transform child in dst.transform) {
			var curSrc = src.Find (child.name);
			if (curSrc) {
				CopyTransformsRecurse (curSrc, child.gameObject);
			}
		}
	}

	private void InitializeData (int hp)
	{
		Debug.Log("initialize character: "+this.gameObject.name +" hp:"+hp);
		initialized = true;
		HP = hp;
	}

	void UpdateData (string id, string team, int hp)
	{
		HP = hp;
		ID = id;
		Team = team;
	}

	
}
