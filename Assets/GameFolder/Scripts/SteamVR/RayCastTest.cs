using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using System.Collections;

public class RayCastTest : MonoBehaviour
{
	public GameObject sphereOne;
	public GameObject sphereTwo;
	public GameObject trackedDevices;

	private uint firstControllerIndex = 0;
	private uint secondControllerIndex = 0;

	private SteamVR vr;
	
	public GameObject player;
	public float scaleAmount;

	private bool firstTriggerPressed = false;
	private bool secondTriggerPressed = false;

	public SteamAttacks firstControllerAttack;
	public SteamAttacks secondControllerAttack;
	
	void Start()
	{
		vr = SteamVR.instance;
		SteamVR_Utils.Event.Listen("device_connected", OnDeviceConnected);
	}

	private void OnDeviceConnected(object[] args)
	{
		var i = (int)args[0];
		var connected = (bool)args[1];
		var isController = (vr.hmd.GetTrackedDeviceClass((uint)i) == Valve.VR.TrackedDeviceClass.Controller);

		if (isController)
		{
			if (firstControllerIndex == 0)
			{
				firstControllerIndex = (uint) i;
			}
			else
			{
				secondControllerIndex = (uint) i;
			}
		}
	}

	void Update()
	{
		// Check for scaling inputs
		if (Input.GetKeyDown(KeyCode.P))
	    {
			StartCoroutine(fadeOutAndIn(1));
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			StartCoroutine(fadeOutAndIn(-1));
		}

		// Check if the player presses the trigger
		firstTriggerPressed = false;
		secondTriggerPressed = false;

		var _event = new VREvent_t ();
		if (SteamVR.instance.hmd.PollNextEvent (ref _event)) 
		{
			if ((EVREventType)_event.eventType == EVREventType.VREvent_ButtonPress) 
			{
				if (_event.data.controller.button == (uint)EVRButtonId.k_EButton_SteamVR_Trigger)
				{
					if (_event.trackedDeviceIndex == firstControllerIndex)
					{
						Debug.Log("First trigger pressed");
						firstTriggerPressed = true;
					}
					else if (_event.trackedDeviceIndex == secondControllerIndex)
					{
						Debug.Log ("Second trigger pressed");
						secondTriggerPressed = true;
					}
				}
			}
		}

		GameObject firstControllerObject = trackedDevices.transform.GetChild (0).gameObject;
		GameObject secondControllerObject = trackedDevices.transform.GetChild (1).gameObject;

		firstControllerAttack.holdFunction (1, firstControllerObject);
		secondControllerAttack.holdFunction (2, secondControllerObject);

		if (Input.GetKeyDown(KeyCode.N))
		{
			firstTriggerPressed = true;
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			secondTriggerPressed = true;
		}
	


		if (firstTriggerPressed)
		{
			firstControllerAttack.releaseFunction(1, firstControllerObject);
		}
		if (secondTriggerPressed)
		{
			secondControllerAttack.releaseFunction(2, secondControllerObject);
		}



		// See where the controllers are being pointed
		controllerLogic (1, sphereOne, firstTriggerPressed);
		controllerLogic (2, sphereTwo, secondTriggerPressed);
	}


	private void controllerLogic(uint controllerIndex, GameObject sphere, bool triggerPressed)
	{	
		RaycastHit hit = getRayHit (controllerIndex);
		if (hit.collider != null)
		{
			sphere.transform.position = hit.point;
			
			ButtonDemoToggle button = (ButtonDemoToggle) hit.collider.gameObject.GetComponent(typeof(ButtonDemoToggle));
			if (button != null)
			{
				if (triggerPressed)
				{
					button.ButtonTurnsOn();
				}
				else
				{
					button.ButtonTurnsOff();
				}
			}
		}
	}

	IEnumerator fadeOutAndIn(float scaleUpOrDown)
	{
		SteamVR_Fade.Start (Color.black, 1);
		yield return new WaitForSeconds(.5f);
		player.transform.localScale += new Vector3 (scaleUpOrDown * scaleAmount, scaleUpOrDown * scaleAmount, scaleUpOrDown * scaleAmount);
		yield return new WaitForSeconds (.5f);
		SteamVR_Fade.Start (Color.clear, 1);
	}


	public RaycastHit getRayHit(uint controllerIndex)
	{
		GameObject thisControllerObject = trackedDevices.transform.GetChild ((int)(controllerIndex - 1)).gameObject;

		RaycastHit hit = new RaycastHit();
		Physics.Raycast (thisControllerObject.transform.position, -1 * thisControllerObject.transform.up, out hit);
		return hit;
	}

	public RaycastHit defensiveRayHit(uint controllerIndex)
	{
		int maskOne = 1 << 10;
		//		int maskTwo = 1 << 11;
		int mask = maskOne;
		GameObject thisControllerObject = trackedDevices.transform.GetChild ((int)(controllerIndex - 1)).gameObject;
		
		Ray controllerOutRay = new Ray (thisControllerObject.transform.position, -1 * thisControllerObject.transform.up);
		RaycastHit hit = new RaycastHit();
		Physics.Raycast (controllerOutRay, out hit, 100f, mask);
		return hit;
	}
}


