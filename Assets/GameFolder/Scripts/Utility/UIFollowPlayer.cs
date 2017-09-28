﻿using UnityEngine;
using System.Collections;

public class UIFollowPlayer : MonoBehaviour 
{
	private GameObject playerCamera;

	// Have different offset positions for the two different types
	public Vector3 offensiveOffset;
	public Vector3 defensiveOffset;

	// Boolean to denote if this is a defensive hud (which doesn't follow player's head exactly)
	public bool isDefensiveHud;

	private Vector3 thisOffset = new Vector3(0.0f, 0.0f, 0.0f);

	public bool isEndGameHud;

	// Use this for initialization
	void Start ()
	{
        playerCamera = GetObjects.instance.getPlayer().gameObject;
        Debug.Log("Camera name: " + playerCamera.name);
		enableUI ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isDefensiveHud)
		{
//			this.transform.localPosition = new Vector3(this.transform.localPosition.x, 0, this.transform.localPosition.z);
			this.transform.rotation = Quaternion.Euler (0.0f, playerCamera.transform.rotation.eulerAngles.y, 0.0f);
		}



		if (isEndGameHud)
		{
			this.transform.rotation = Quaternion.Euler (0.0f, 190.0f, 0.0f);
			this.transform.position = playerCamera.transform.position + offensiveOffset;
		}
		else
		{
			this.transform.position = playerCamera.transform.position + thisOffset;
		}
	}

	public void enableUI()
	{
		this.gameObject.SetActive (true);

		thisOffset = playerCamera.transform.rotation * offensiveOffset;


		if (isEndGameHud)
		{
			this.transform.rotation = Quaternion.Euler (0.0f, 190.0f, 0.0f);
		}
		else
		{
			this.transform.rotation = Quaternion.Euler (0.0f, playerCamera.transform.rotation.eulerAngles.y, 0.0f);
		}
	}

	public void disableUI()
	{
		this.gameObject.SetActive (false);
	}
}
