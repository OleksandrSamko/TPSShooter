using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FPSController))]


public class FPSInputController : MonoBehaviour
{
	public FPSController FPSmotor;

	void Start ()
	{
		FPSmotor = GetComponent<FPSController> ();	
		Application.targetFrameRate = 60;
	}

	void Awake ()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update ()
	{
		if (FPSmotor == null || FPSmotor.character == null)
			return; 
//		Debug.Log("FPSmotor "+FPSmotor);
		//FPSItemEquipment FPSitem = null;

		FPSmotor.Move (new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical")));
		// jump input
		FPSmotor.Jump (Input.GetButton ("Jump"));

		// sprint input
		if (Input.GetKey (KeyCode.LeftShift)) {
			FPSmotor.Boost (1.4f);	
//			Debug.Log ("Boost ");
		}

		ItemEquipment actionItem = GetComponentInChildren<ItemEquipment>(); 
		
		if (Cursor.lockState == CursorLockMode.Locked) {
			// aim input work only when mouse is locked
			FPSmotor.Aim (new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y")));
		
			// fire input
			if (Input.GetButton ("Fire1")) {
				// press trigger to fire
				//	Debug.Log("Fire1 ");
				actionItem.Trigger ();
			} else {
				// relesed trigger.
				actionItem.OnTriggerRelease ();
			}
			// fire 2 input e.g. Zoom
			if (Input.GetButtonDown ("Fire2")) {
				// press trigger 2
				//				Debug.Log("Fire2 ");
				actionItem.Trigger2 ();
			} else {
				// release trigger 2
				actionItem.OnTrigger2Release ();
			}
		}
			
		// interactive input e.g. pickup item
		if (Input.GetKeyDown (KeyCode.F)) {
		}
		// reload input
		if (Input.GetKeyDown (KeyCode.R)) {
			actionItem.Reload ();
		}

	}
}
