﻿using UnityEngine;
using System.Collections;

public class RayCastToPress : MonoBehaviour
{
	public GameObject thisCamera;
	public ButtonDemoToggle buttonToTurnOn;
	public float timeToActivate;
	private float timeHeld;

	// Use this for initialization
	void Start () 
	{
		timeHeld = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit ray = getRayHit ();

		if (ray.collider != null) 
		{
			print ("Object hit: " + ray.collider.gameObject.name);
			if (ray.collider.gameObject == buttonToTurnOn.gameObject)
			{
				timeHeld += Time.deltaTime;
				if (timeHeld > timeToActivate)
				{
					buttonToTurnOn.ButtonTurnsOn();
				}
			}
		}
		else
		{
			timeHeld = 0;
		}
	}



	private RaycastHit getRayHit()
	{
		int maskOne = 1 << 5;
		int mask = maskOne;
		Ray ray = new Ray (thisCamera.transform.position, thisCamera.transform.forward);
		RaycastHit hit;
		Physics.Raycast (ray, out hit, 100f, mask);
		return hit;
	}
}