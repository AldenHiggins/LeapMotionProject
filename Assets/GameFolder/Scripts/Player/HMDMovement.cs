using UnityEngine;
using System.Collections;

public class HMDMovement : MonoBehaviour 
{
	public GameObject camera;

	private Vector3 initialPosition;

	private CharacterController controller;

	public float moveSpeed;

	// Use this for initialization
	void Start ()
	{
		controller = gameObject.GetComponent<CharacterController> ();
		initialPosition = camera.transform.localPosition;
		StartCoroutine (delayedInitialPosition());
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Press button to "reset" base position
		if (Input.GetKeyDown (KeyCode.R))
		{
			resetPosition();
		}


//		Vector3 vectorFromInitial = camera.transform.localPosition - initialPosition;
//		float totalDistance = Vector3.Distance (camera.transform.localPosition, initialPosition);
//
//
//		Vector3 cameraForward = camera.transform.forward;
//		Vector3 perpendicularVector = new Vector3(cameraForward.z, cameraForward.y, -1 * cameraForward.x);
//
//		if (totalDistance > .13)
//		{
//			vectorFromInitial.y = 0;
//			Vector3 playerSpeed = vectorFromInitial.normalized * .03f;
////			gameObject.transform.position += new Vector3(playerSpeed.z, 0.0f, -1.0f * playerSpeed.x);
////			controller.Move (new Vector3(playerSpeed.z, 0.0f, -1.0f * playerSpeed.x));
////			Vector3 moveVector = playerSpeed.x * perpendicularVector;
////			moveVector += playerSpeed.z * cameraForward;
//
//			Vector3 moveVector = -1 * Vector3.Dot (playerSpeed, perpendicularVector) * perpendicularVector;
//			moveVector -= Vector3.Dot (playerSpeed, cameraForward) * cameraForward;
//
//			// Scale the move vector by the change in time to ensure even velocity across frame rates
//			moveVector *= Time.deltaTime;
//			moveVector *= 100;
//
//			controller.Move (moveVector);
//		}

		Vector3 vectorFromInitial = camera.transform.localPosition - initialPosition;
		vectorFromInitial.y = 0;
		float totalDistance = Vector3.Magnitude(vectorFromInitial);
		
		
		Vector3 cameraForward = camera.transform.forward;
		Vector3 perpendicularVector = new Vector3(cameraForward.z, cameraForward.y, -1 * cameraForward.x);
		
		if (totalDistance > .13)
		{
			vectorFromInitial.y = 0;
			Vector3 playerSpeed = vectorFromInitial * moveSpeed;
			//			gameObject.transform.position += new Vector3(playerSpeed.z, 0.0f, -1.0f * playerSpeed.x);
			//			controller.Move (new Vector3(playerSpeed.z, 0.0f, -1.0f * playerSpeed.x));
			//			Vector3 moveVector = playerSpeed.x * perpendicularVector;
			//			moveVector += playerSpeed.z * cameraForward;
			
			Vector3 moveVector = -1 * Vector3.Dot (playerSpeed, perpendicularVector) * perpendicularVector;
			moveVector -= Vector3.Dot (playerSpeed, cameraForward) * cameraForward;
			
			// Scale the move vector by the change in time to ensure even velocity across frame rates
			moveVector *= Time.deltaTime;
			moveVector *= 100;
			
			controller.Move (moveVector);
		}
	
	}

	public void resetPosition()
	{
		initialPosition = camera.transform.localPosition;
	}

	IEnumerator delayedInitialPosition()
	{
		yield return new WaitForSeconds(.5f);
		initialPosition = camera.transform.localPosition;
	}
}
