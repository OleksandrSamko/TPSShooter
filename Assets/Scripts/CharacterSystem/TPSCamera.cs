using UnityEngine;
using System.Collections;

public class TPSCamera : MonoBehaviour
{
//main camera has 0.15 x offset
	public Camera MainCamera;
	public float offset;
	public Transform target;
	private FPSController fpscontroller;
	public float targetHeight = 1.2f;
	public float distance = 4.0f;
	public float maxDistance = 6;
	public float minDistance = 1.0f;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -10;
	public float yMaxLimit = 70;
	private float x = 20.0f;
	private float y = 0.0f;
	public Quaternion aim;
	public float aimAngle = 8;
	public bool  lockOn = false;

	public float zoomOut=65;
	public float zoomIn=65;

	private RaycastHit hit;
	
	void  Start ()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	public void Zoom (bool activate)
	{
		if (activate) {
			MainCamera.fieldOfView = zoomIn;
		} else {
			MainCamera.fieldOfView = zoomOut;
		}
	}

	public void SetTarget (Transform inTarget)
	{
		target = inTarget;
		fpscontroller = target.GetComponent<FPSController> ();
	}

	void  LateUpdate ()
	{

		//no time
		if (Time.timeScale == 0.0f) {
			return;
		}
		//no target
		if(!target){
			return;
		}

		Quaternion rotation;
		// Rotate Camera only if mouse dont lock
		if (Cursor.lockState == CursorLockMode.Locked) {
			x += fpscontroller.mouseDirection.x * xSpeed * 0.02f;
			y -= fpscontroller.mouseDirection.y * ySpeed * 0.02f;
			
			y = ClampAngle (y, yMinLimit, yMaxLimit);

			rotation = Quaternion.Euler (y, x, 0);
			transform.rotation = rotation;
			aim = Quaternion.Euler (y - aimAngle, x, 0);
		
			//Rotate Target
			if (lockOn) {
				target.transform.rotation = Quaternion.Euler (0, x, 0);
			}
			//dont rotate
		}else{
			rotation = transform.rotation;
		}

		//Camera Position
		Vector3 positiona = target.position - (rotation * Vector3.forward * distance + new Vector3 (0.0f, -targetHeight, 0.0f));
		transform.position = positiona;
		
		Vector3 trueTargetPosition = target.transform.position - new Vector3 (0.0f, -targetHeight, 0.0f);
		
		if (Physics.Linecast (trueTargetPosition, MainCamera.transform.position, out hit)) {
			if (hit.transform.tag == "Scene") {
				float tempDistance = Vector3.Distance (trueTargetPosition, hit.point) - 0.28f;
				Vector3 v = new Vector3 (offset, 0f, 0f) + Vector3.forward * tempDistance;
				positiona = target.position - (rotation * v + new Vector3 (0, -targetHeight, 0));
				transform.position = positiona;
			}
		}

	}
	
	static float  ClampAngle (float angle, float min, float max)
	{
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
		
	}
}
