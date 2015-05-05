using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
	public float rotationSpeed;
	public Vector3 rotationAxis;

	void Update()
	{
		Vector3 newRotationAxis = gameObject.transform.rotation * rotationAxis;
		gameObject.transform.RotateAround (gameObject.transform.position, newRotationAxis, rotationSpeed);
	}


}

