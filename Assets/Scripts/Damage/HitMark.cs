using UnityEngine;
using System.Collections;

public struct DamagePackage{
	public Vector3 Position;
	public Vector3 Direction;
	public Vector3 Normal;
	public int Damage;
	public string ID;
	public string Team;
}

public class HitMark : MonoBehaviour
{

	public DamageManager damageManage;
	public GameObject HitFX;
	public float DamageMult = 1;

	void Start ()
	{
		if (this.transform.root) {
			damageManage = this.transform.root.GetComponent<DamageManager> ();
		} else {
			damageManage = this.transform.GetComponent<DamageManager> ();
		}
	}
	
	public void OnHit (DamagePackage pack)
	{
		if (damageManage) {
			damageManage.ApplyDamage ((int)((float)pack.Damage * DamageMult),pack.Direction,pack.ID,pack.Team);
		}
		ParticleFX (pack.Position, pack.Normal);
		
	}
	
	public void ParticleFX (Vector3 position, Vector3 normal)
	{
		if (HitFX) {
			GameObject fx = (GameObject)GameObject.Instantiate (HitFX, position, Quaternion.identity);
			fx.transform.forward = normal;
			GameObject.Destroy (fx, 3);
		}
	}
}
