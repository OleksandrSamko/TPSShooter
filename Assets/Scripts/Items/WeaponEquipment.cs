
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class WeaponEquipment : ItemEquipment
{
	
	private CharacterSystem character;
	private FPSController fpsController;
	private float timeTemp;
	private AudioSource audioSource;
	private Animator animator;
//stats
	public bool HoldFire = true;
	public float FireRate = 0.09f;
	public float Spread = 20;
	public int BulletNum = 1;
	public int Damage = 10;
	public float Force = 10;
	public Vector2 KickPower = Vector2.zero;
	public int MaxPenetrate = 1;
	public float Distance = 100;
	public int Ammo = 30;
	public int AmmoMax = 30;
	public int AmmoHave = 0;
	public int reloadTime = 1;
	private bool reloading;

	public AudioClip SoundFire;
	public AudioClip SoundReload;
	public AudioClip SoundReloadComplete;
	public AudioClip[] DamageSound;

	public GameObject MuzzleFX;
	public Transform MuzzlePoint;
	public int UsingType = 0;
	public bool InfinityAmmo;

	public float AnimationSpeed = 1;
	private float animationSpeedTemp = 1;

	public float panicfire = 0;
	public float PanicFireMult = 0.1f;
	public float SpreadZoomMult = 1;
	public bool HideWhenZoom = false;
	public GameObject ProjectileFX;

	void Start ()
	{
		reloading = false;
		animator = this.GetComponent<Animator> ();
		audioSource = this.GetComponent<AudioSource> ();

		if (this.transform.root) {
			character = this.transform.root.GetComponent<CharacterSystem> ();
			fpsController = this.transform.root.GetComponent<FPSController> ();
			if (character == null)
				character = this.transform.root.GetComponentInChildren<CharacterSystem> ();
			if (fpsController == null)
				fpsController = this.transform.root.GetComponentInChildren<FPSController> ();
		} else {
			character = this.transform.GetComponent<CharacterSystem> ();
			fpsController = this.transform.GetComponent<FPSController> ();
		}
		
		if (character)
			character.DamageSound = DamageSound;
		
		if (fpsController)
			fpsController.zooming = false;
		
		Hide (true);
		timeTemp = Time.time;

		if (animator) {
			animationSpeedTemp = animator.speed;
		}
	}
	
	public override void Trigger ()
	{
		if (!HoldFire && OnFire1)
			return;
		
		if (character && fpsController) {
			if (!reloading && Time.time > timeTemp + FireRate) {
				Shoot ();
				timeTemp = Time.time;
			}
		}
		base.Trigger ();
	}

	public override void OnTriggerRelease ()
	{
		base.OnTriggerRelease ();
	}

	public override void Trigger2 ()
	{
		fpsController.Zoom ();
		base.Trigger2 ();
	}

	public override void OnTrigger2Release ()
	{
		base.OnTrigger2Release ();
	}
	
	public void Shoot ()
	{

		if (Ammo <= 0 && !InfinityAmmo)
			return;

		OnAction ();

		if (character != null) {
			character.AttackAnimation (UsingType);
		}
		if (animator) {
			animator.speed = AnimationSpeed;
			animator.SetTrigger ("shoot");
		}
		
	}


	public override void Reload ()
	{
		if (Ammo >= AmmoMax || Ammo <= 0 && AmmoHave <= 0)
			return;

		//reload wait
		if (!reloading) {
			StartCoroutine(ReloadComplete());

			if (audioSource && SoundReload) {
				audioSource.PlayOneShot (SoundReload);
			}
		}
		
		if (animator)
			animator.SetTrigger ("reloading");	
		reloading = true;

		base.Reload ();
	}
	
	public override IEnumerator ReloadComplete ()
	{
		int ammoused = AmmoMax - Ammo;
		if (AmmoHave < ammoused) {
			ammoused = AmmoHave;
		}
		yield return new WaitForSeconds(reloadTime);
		
		Ammo += ammoused;
		AmmoHave -= ammoused;

		if (audioSource && SoundReloadComplete) {
			audioSource.PlayOneShot (SoundReloadComplete);
		}	
		//end reload
		reloading = false;
	}
	
	private float spreadmult;

	void Update ()
	{
		if (!reloading && Ammo <= 0) {
			Reload ();
		}
		
		//	Hide (true);
		spreadmult = 1;
		//	if (HideWhenZoom) {
		//		if (fpsController && fpsController.zooming) {
		//			Hide (false);
		//			spreadmult = SpreadZoomMult;
		//		}
		//	}
		
		if (panicfire < 0.01f)
			panicfire = 0;
		panicfire += (0 - panicfire) * 5 * Time.deltaTime;
		
		if (animator)
			animator.SetInteger ("shoot_type", UsingType);

	}
		
	public void PlayFireSound ()
	{
		if (audioSource && SoundFire) {
			audioSource.PlayOneShot (SoundFire);	
		}
	}

	void OnGUI ()
	{
		if (InfinityAmmo)
			return;

		GUI.skin.label.alignment = TextAnchor.LowerRight;
		GUI.skin.label.fontSize = 35;
		GUI.Label (new Rect (Screen.width - 230, Screen.height - 90, 200, 60), Ammo + "/" + AmmoHave);		
	
	}
	
	public override void OnAction ()
	{
//		Debug.Log(this+" OnAction");

		if (animator)
			animator.speed = animationSpeedTemp;
		if (Ammo > 0 || InfinityAmmo) {
			if (!InfinityAmmo)
				Ammo -= 1;
		
			if (BulletNum <= 0)
				BulletNum = 1;
		
			Vector3[] dirs = new Vector3[BulletNum];
		
			//muzzle
			if (!MuzzlePoint) {
				MuzzlePoint = this.transform;
			}
			//effects sound
			PlayFireSound();

			GameObject muzzleObj = GameObject.Instantiate (MuzzleFX, MuzzlePoint);
			Destroy (muzzleObj, 3);

			for (int i=0; i<dirs.Length; i++) {
			
			
				if (dirs.Length <= 1)
					panicfire = 1;
//				Debug.Log(fpsController.FPSCamera);
				if (fpsController) {
					dirs [i] = (fpsController.tpsCamera.MainCamera.transform.forward + (new Vector3 (Random.Range (-Spread + (int)panicfire, Spread + (int)panicfire) * 0.001f, Random.Range (-Spread + (int)panicfire, Spread + (int)panicfire) * 0.001f, Random.Range (-Spread + (int)panicfire, Spread + (int)panicfire) * 0.001f) * spreadmult));
				}
				if (ProjectileFX && fpsController) {
					GameObject.Instantiate (ProjectileFX, fpsController.tpsCamera.MainCamera.transform.position + (dirs [i] * 5), Quaternion.LookRotation (dirs [i]));	
				}
				dirs [i] *= Force;
			}
		
			if (fpsController)
				fpsController.Kick (KickPower);
					
			if (character != null) {
				character.DoDamage (fpsController.tpsCamera.MainCamera.transform.position, dirs, Damage, Distance, MaxPenetrate, character.ID, character.Team);
			}

			panicfire += PanicFireMult;
		}

		base.OnAction ();
		
	}
	
	void MuzzlePosition ()
	{
		
	}
}



