using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterSystem
{

	void Awake ()
	{
		SetupAwake ();	
	}

	public override void SetupAwake(){
		base.SetupAwake();
	}

	void Start ()
	{	
		if(animator)
			animator.SetInteger ("Shoot_Type", AttackType);
	}

	void Update ()
	{
		UpdateFunction ();
	}
	
	public override void PlayMoveAnimation (float magnitude)
	{
		if (animator) {
			if (magnitude > 0.4f) {
				animator.SetInteger ("LowerState", 1);
			} else {
				animator.SetInteger ("LowerState", 0);
			}
		}
	
		base.PlayMoveAnimation (magnitude);
	}

	public override void PlayAttackAnimation (bool attacking, int attacktype)
	{
		if (animator) {
			if (attacking) {
				animator.SetTrigger ("Shoot");
			}
			animator.SetInteger ("Shoot_Type", attacktype);
		}
		base.PlayAttackAnimation (attacking, attacktype);
	}

	public override void OnThisThingDead ()
	{
		//change pls
		FindObjectOfType<InGameMenu>().ShowMenu();

		base.OnThisThingDead ();
	}
}
