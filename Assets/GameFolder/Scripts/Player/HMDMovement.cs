using UnityEngine;
using System.Collections;

public class HMDMovement : MonoBehaviour 
{
	public GameObject camera;

	private Vector3 initialPosition;

	private CharacterController controller;

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
			initialPosition = camera.transform.localPosition;
		}


		Vector3 vectorFromInitial = camera.transform.localPosition - initialPosition;
		float totalDistance = Vector3.Distance (camera.transform.localPosition, initialPosition);


		print ("Initial position: " + initialPosition);
		print ("Distance from initial: " + vectorFromInitial);
		print ("Camera normal: " + camera.transform.forward);
		Vector3 cameraForward = camera.transform.forward;
		Vector3 perpendicularVector = new Vector3(cameraForward.z, cameraForward.y, -1 * cameraForward.x);
		print ("Camera right: " + perpendicularVector);
//		print ("Total distance: " + totalDistance);

		if (totalDistance > .13)
		{
			vectorFromInitial.y = 0;
			Vector3 playerSpeed = vectorFromInitial.normalized * .03f;
//			gameObject.transform.position += new Vector3(playerSpeed.z, 0.0f, -1.0f * playerSpeed.x);
//			controller.Move (new Vector3(playerSpeed.z, 0.0f, -1.0f * playerSpeed.x));
			Vector3 moveVector = playerSpeed.x * perpendicularVector;
			moveVector += playerSpeed.z * cameraForward;
			controller.Move (moveVector);
		}
	
	}

	IEnumerator delayedInitialPosition()
	{
		yield return new WaitForSeconds(.5f);
		initialPosition = camera.transform.localPosition;
	}
}
