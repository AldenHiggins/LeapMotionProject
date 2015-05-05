using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
	public float rotationSpeed;
	public Vector3 rotationAxis;

	void Update()
	{
		gameObject.transform.RotateAround (gameObject.transform.position, rotationAxis, rotationSpeed);
	}


}

