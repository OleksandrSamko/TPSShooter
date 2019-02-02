using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterMotor))]
[RequireComponent (typeof(FPSRayActive))]

public class CharacterSystem : DamageManager
{
	[HideInInspector]
	public string CharacterKey = "";
	[HideInInspector]
	public Animator animator;
	[HideInInspector]
	public FPSRayActive rayActive;
	[HideInInspector]
	public CharacterController controller;
	[HideInInspector]
	public CharacterMotor Motor;
	public bool Sprint;
	public float MoveSpeed = 0.7f;
	public float MoveSpeedMax = 5;
	public float TurnSpeed = 5;
	public float PrimaryWeaponDistance = 1;
	public int PrimaryItemType;
	public int AttackType = 0;
	public int Damage = 2;
	public float DamageLength = 1;
	public int Penetrate = 1;
	public AudioClip[] DamageSound;
	public AudioClip[] SoundIdle;
	[HideInInspector]
	public float spdMovAtkMult = 1;

	void Awake ()
	{
		SetupAwake ();
	}

	public virtual void SetupAwake ()
	{
		Motor = this.GetComponent<CharacterMotor> ();
		controller = this.GetComponent<CharacterController> ();
		Audiosource = this.GetComponent<AudioSource> ();
		animator = this.GetComponent<Animator> ();
		if (!animator) {
			animator = this.GetComponentInChildren<Animator> ();
		}

		rayActive = this.GetComponent<FPSRayActive> ();
		spdMovAtkMult = 1;
	}

	void Update ()
	{
		UpdateFunction ();
	}

	public void UpdateFunction ()
	{
		Motor.ID = ID;
		UpdatePosition ();
		DamageUpdate ();
	}

	public virtual void PlayAttackAnimation (bool attacking, int attacktype)
	{
		if (animator) {
			if (attacking) {
				animator.SetTrigger ("Shoot");
			}
			animator.SetInteger ("Shoot_Type", attacktype);
		}
	}

	public virtual void PlayMoveAnimation (float magnitude)
	{
		if (animator) {
			if (magnitude > 0.4f) {
				animator.SetInteger ("LowerState", 1);
			} else {
				animator.SetInteger ("LowerState", 0);
			}
		}
	}

	public void MoveAnimation ()
	{
		PlayMoveAnimation (Motor.OjectVelocity.magnitude);
	}

	public void MoveTo (Vector3 dir)
	{
		float speed = MoveSpeed;
		if (Sprint)
			speed = MoveSpeedMax;
		
		Move (dir * speed * spdMovAtkMult);
		MoveAnimation ();
	}

	public void MoveToPosition (Vector3 position)
	{
		float speed = MoveSpeed;
		if (Sprint)
			speed = MoveSpeedMax;
		Vector3 direction = (position - transform.position);
		direction = Vector3.ClampMagnitude (direction, 1);
		direction.y = 0;
		Move (direction.normalized * speed * direction.magnitude * spdMovAtkMult);
		if (direction != Vector3.zero) {
			Quaternion newrotation = Quaternion.LookRotation (direction);
			transform.rotation = Quaternion.Slerp (transform.rotation, newrotation, Time.deltaTime * TurnSpeed * direction.magnitude);
		}
		MoveAnimation ();
	}

	public void UpdateTransform ()
	{
		if (this.transform.parent == null) {
			this.transform.position = Vector3.Lerp (this.transform.position, this.transform.position, 0.5f);
		}
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, this.transform.rotation, 0.5f);
		//Motor.OjectVelocity = velocity;
		MoveAnimation ();	
	}

	public void UpdatePosition ()
	{
		UpdateTransform ();
	}

	public void AttackAnimation (int attacktype = 0)
	{
		AttackType = attacktype;
		PlayAttackAnimation (true, attacktype);
	}

	public void AttackTo (Vector3 direction, int attacktype)
	{
		AttackType = attacktype;
		PlayAttackAnimation (true, attacktype);
	}

	public void UpdateAnimationState ()
	{
		PlayAttackAnimation (false, AttackType);
	}

	public void DoDamage (Vector3 origin, Vector3[] direction, int damage, float distance, int penetrate, string id, string team)
	{
		//Debug.Log("do damage: "+damage);
		if (rayActive) {
			if (rayActive.ShootRay (origin, direction, damage, distance, penetrate, id, team))
				PlayDamageSound ();
		}
	}


	public void Move (Vector3 directionVector)
	{
		if (Motor) {
			Motor.inputMoveDirection = directionVector;
		}
	}

	public void PlayIdleSound ()
	{
		if (Audiosource && SoundIdle.Length > 0) {
			Audiosource.PlayOneShot (SoundIdle [Random.Range (0, SoundIdle.Length)]);	
		}
	}

	public void PlayDamageSound ()
	{
		if (Audiosource && DamageSound.Length > 0) {
			Debug.Log ("DamageSound[0] " + DamageSound [0]);
			Audiosource.PlayOneShot (DamageSound [Random.Range (0, DamageSound.Length)]);	
		}
	}

	public virtual void OnThisThingDead ()
	{
		//hmmm count player too
		UnitGlobal.scoreManager.AddScore (1);
	}

}
