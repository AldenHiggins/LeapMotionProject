using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private GameObject playerCamera;

	// Use this for initialization
	void Start ()
    {
        playerCamera = GetObjects.instance.getCamera();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(playerCamera.transform);	
	}
}
