using UnityEngine;
using System.Collections;

public class HMDMovement : MonoBehaviour 
{
	public GameObject camera;

	private Vector3 initialPosition;

	// Use this for initialization
	void Start ()
	{
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
		print ("Total distance: " + totalDistance);

		if (totalDistance > .1)
		{
			vectorFromInitial.y = 0;
			Vector3 playerSpeed = vectorFromInitial.normalized * .03f;
			gameObject.transform.position += new Vector3(playerSpeed.z, 0.0f, -1.0f * playerSpeed.x);
		}
	
	}

	IEnumerator delayedInitialPosition()
	{
		yield return new WaitForSeconds(.5f);
		initialPosition = camera.transform.localPosition;
	}
}
