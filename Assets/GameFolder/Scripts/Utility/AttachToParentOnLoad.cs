using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToParentOnLoad : MonoBehaviour
{ 
	// Use this for initialization
	void Start ()
    {
        transform.parent = GetObjects.getGame().gameObject.transform.parent;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
