using UnityEngine;
using System.Collections;

public class DontRotate : MonoBehaviour 
{	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = Quaternion.identity;		
	}
}
