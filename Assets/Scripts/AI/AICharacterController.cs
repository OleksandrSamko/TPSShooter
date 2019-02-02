
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterSystem))]

public class AICharacterController : MonoBehaviour
{
	public string[] TargetTag = { "Player" };
	public GameObject ObjectTarget;
	[HideInInspector]
	public Vector3 PositionTarget;
	[HideInInspector]
	public CharacterSystem character;
	[HideInInspector]
	public float DistanceAttack = 2;
	public float DistanceMoveTo = 20;
	public float TurnSpeed = 10.0f;
	public float PatrolRange = 10;
	[HideInInspector]
	public Vector3 positionTemp;
	//[HideInInspector]
	public int aiTime = 0;
	//[HideInInspector]
	public int aiState = 0;
	private float attackTemp = 0;
	public float AttackDelay = 0.5f;
	//	public float LifeTime = 30;
	public float IdleSoundDelay = 10;
	//	private float lifeTimeTemp = 0;
	private float jumpTemp, jumpTime, soundTime, soundTimeDuration;
	public int JumpRate = 20;


	void Start ()
	{
		character = gameObject.GetComponent<CharacterSystem> ();
		positionTemp = this.transform.position;
		aiState = 0;
		attackTemp = Time.time;
		jumpTemp = Time.time;
		soundTime = Time.time;
		soundTimeDuration = Random.Range (0, IdleSoundDelay);
		character.ID = "";
	}

	public void Attack (Vector3 targetDirectiom)
	{
		if (Time.time > attackTemp + AttackDelay) {
			Vector3[] dirs = new Vector3[1];
			dirs [0] = targetDirectiom.normalized;
			character.DoDamage (this.transform.position, dirs, character.Damage, character.DamageLength, character.Penetrate, "", character.Team);
			character.AttackAnimation ();
			attackTemp = Time.time;	
		}
	}

	void Update ()
	{
		if (character == null)
			return;
		
		// random play an idle sound.
		if (Time.time > soundTime + soundTimeDuration) {
			character.PlayIdleSound ();	
			soundTimeDuration = Random.Range (0, IdleSoundDelay);
			soundTime = Time.time;
		}
			
		// get attack distance from primary weapon.
		DistanceAttack = character.PrimaryWeaponDistance;	

		float distance = Vector3.Distance (PositionTarget, this.gameObject.transform.position);
		Vector3 targetDirectiom = (PositionTarget - this.transform.position);
		Quaternion targetRotation = this.transform.rotation;
		float str = TurnSpeed * Time.time;
		// rotation to look at a target
		if (targetDirectiom != Vector3.zero) {
			targetRotation = Quaternion.LookRotation (targetDirectiom);
			targetRotation.x = 0;
			targetRotation.z = 0;
			transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
		}
			
		// if Target is exist
		if (ObjectTarget != null) {
			//	lifeTimeTemp = Time.time;
			PositionTarget = ObjectTarget.transform.position;
			if (aiTime <= 0) {
				//aiState = Random.Range (0, 4);
				aiTime = Random.Range (10, 100);
			} else {
				aiTime--;
			}
				
			//Debug.Log(distance + "<="+ DistanceAttack);
			// attack only distance.
			if (distance <= DistanceAttack) {
//				Debug.Log("Attack : "+targetDirectiom);
				Attack (targetDirectiom);
			} else {
				if (distance <= DistanceMoveTo) {
					// rotation facing to a target.
					transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
				} else {
					// if target is out of distance
					ObjectTarget = null;
					if (aiState == 0) {
						aiState = 1;
						aiTime = Random.Range (10, 500);
						PositionTarget = positionTemp + new Vector3 (Random.Range (-PatrolRange, PatrolRange), 0, Random.Range (-PatrolRange, PatrolRange));
					}
				}
			}

		} else {
	
			float length = float.MaxValue;

			for (int t = 0; t < TargetTag.Length; t++) {
				// Finding all the targets by Tags.
				GameObject[] targets = (GameObject[])GameObject.FindGameObjectsWithTag (TargetTag [t]);
				if (targets != null && targets.Length > 0) {
					for (int i = 0; i < targets.Length; i++) {
						float distancetargets = Vector3.Distance (targets [i].gameObject.transform.position, this.gameObject.transform.position);
						if ((distancetargets <= length && (distancetargets <= DistanceMoveTo || distancetargets <= DistanceAttack)) && ObjectTarget != targets [i].gameObject) {
							length = distancetargets;
							ObjectTarget = targets [i].gameObject;
						}
					}
				}
			}
			if (aiState == 0) {
				// AI state == 0 mean AI is free, so moving to anywhere
				aiState = 1;
				aiTime = Random.Range (10, 200);
				PositionTarget = positionTemp + new Vector3 (Random.Range (-PatrolRange, PatrolRange), 0, Random.Range (-PatrolRange, PatrolRange));
			}
			if (aiTime <= 0) {
				// random AI state
				aiState = Random.Range (0, 4);
				aiTime = Random.Range (10, 200);
			} else {
				aiTime--;
			}
		}
		character.MoveToPosition (PositionTarget);
			
	}
}