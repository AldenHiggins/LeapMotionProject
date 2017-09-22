using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMovingObjectsOnLoad : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        GetObjects.getMovingObjectsContainer().transform.position = Vector3.zero;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
