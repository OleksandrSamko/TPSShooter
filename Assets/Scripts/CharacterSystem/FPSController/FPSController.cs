using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(FPSInputController))]
[AddComponentMenu("Character/FPS Input Controller")]

public class FPSController : MonoBehaviour
{
	public GameObject cameraPrefab;
	[HideInInspector]
	public CharacterSystem		character;
	[HideInInspector]
	public CharacterMotor motor;
	[HideInInspector]
	public Vector3
		inputDirection;
	public Vector2 mouseDirection;
	public GameObject TPSViewPart;
	private Quaternion originalRotation;
	private Vector2 kickPower;
	public TPSCamera tpsCamera; 
	[HideInInspector]
	public bool
		zooming = false;
	Quaternion xyQuaternion;

	void Start ()
	{
		character = gameObject.GetComponent<CharacterSystem> ();
				
		motor = GetComponent<CharacterMotor> ();
		if (GetComponent<Rigidbody> ())
			GetComponent<Rigidbody> ().freezeRotation = true;
		
		originalRotation = transform.localRotation;
//		Debug.Log ("TPSViewPart : " + TPSViewPart);
	}

	void Awake ()
	{
		if (!TPSViewPart) {
			CreateCamera ();
		}

	}

	public void Zoom ()
	{
		if(zooming){
			zooming = false;
		}else{
			zooming = true;
		}
		tpsCamera.Zoom(zooming);
	}
	
	public void Kick (Vector2 power)
	{
		// kick function e.g when shoot a gun, camera kick up
		kickPower = power;
	}
	
	public void HideGun (bool visible)
	{

	}

	public void Boost (float mult)
	{
		motor.boostMults = mult;
	}
	
	float climbDirection;

	public void Climb (float speed)
	{
		motor.Climb (speed);	
	}
	
	public void Move (Vector3 directionVector)
	{
		// move along with direction.
		if (character == null)
			return;
		
		inputDirection = directionVector;
		if (directionVector != Vector3.zero) {
			var directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			directionLength = Mathf.Min (1, directionLength);
			directionLength = directionLength * directionLength;
			directionVector = directionVector * directionLength;
		}
		
		Quaternion rotation = transform.rotation;
		
		if (TPSViewPart) {
			rotation = TPSViewPart.transform.rotation;
		}
		Vector3 angle = rotation.eulerAngles; 
		angle.x = 0;
		angle.z = 0;
		rotation.eulerAngles = angle;
		character.MoveTo (rotation * directionVector);
	}

	public void Jump (bool jump)
	{
		// jump funtion
		motor.inputJump = jump;
	}
	
	public void Aim (Vector2 direction)
	{
		// aim function
		mouseDirection = direction;
//		Debug.Log(mouseDirection);
	}
	
	void Update ()
	{
		if (Cursor.lockState == CursorLockMode.None || character == null)
			return;

		// sprint speed.
		motor.boostMults += (1 - motor.boostMults) * Time.deltaTime;
		// camera kick
		kickPower.y += (0 - kickPower.y) / 20f;
		kickPower.x += (0 - kickPower.x) / 20f;
	}

	public void CreateCamera ()
	{
		TPSViewPart = GameObject.FindGameObjectWithTag("MainCamera");
		if(!TPSViewPart){
			TPSViewPart = Instantiate (cameraPrefab, Vector3.zero, Quaternion.identity);
		}
		tpsCamera = TPSViewPart.GetComponent<TPSCamera> ();
		tpsCamera.SetTarget (transform);
//		Debug.Log ("Create camera: "+TPSViewPart);
	}

	public void Stun (float val)
	{
		// kick function e.g when shoot a gun, camera kick up
		kickPower.y = val;
	}

}
