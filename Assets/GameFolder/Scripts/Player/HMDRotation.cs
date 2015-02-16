using UnityEngine;
using System.Collections;

public class HMDRotation : MonoBehaviour 
{
	public GameObject cameraCenter;
	
	private Quaternion referenceRotation;
	private Quaternion currentRotation;
	public float turnSensitivityMultiplier; //Use this to multiply the rotation amount 
	public float turnAngleThreshold;
	public float overCorrect;

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{
		referenceRotation = transform.rotation;
		currentRotation = cameraCenter.transform.rotation;

		//Debug.Log ("Reference angle in Quat is: " + referenceRotation.y);
		//Debug.Log ("Reference angle in Eulr is: " + referenceRotation.eulerAngles.y);
		//Debug.Log ("Current   angle in Quat is: " + currentRotation.y);
		//Debug.Log ("Current   angle in Eulr is: " + currentRotation.eulerAngles.y);

		float yDifference = GetAngleDifference (currentRotation.eulerAngles.y, referenceRotation.eulerAngles.y);
		//float yDifference = currentRotation.eulerAngles.y - referenceRotation.eulerAngles.y;
		//yDifference = (yDifference + 180) % 360 - 180;
		//Debug.Log ("Angle offset is: " + yDifference);

		if (Mathf.Abs (yDifference) > turnAngleThreshold) 
		{
			Vector3 euler = transform.rotation.eulerAngles;
			euler.y += yDifference * turnSensitivityMultiplier * Time.deltaTime;

			euler.y = euler.y % 360;

			transform.rotation = Quaternion.Euler(euler);

			//referenceRotation.eulerAngles.Set(referenceRotation.eulerAngles.x, referenceRotation.eulerAngles.y + euler.y, referenceRotation.eulerAngles.z);
		}
	}

	float GetAngleDifference(float newAngle, float oldAngle)
	{
		float angleDiff = newAngle - oldAngle;
		if (angleDiff > 0) 
		{
			if (angleDiff > 180) {
				angleDiff -= 360;
			}
		} 
		else if (angleDiff < 0) 
		{
			if (Mathf.Abs(angleDiff) > 180) {
				angleDiff += 360;
			}

		}

		return angleDiff;

	}

	
}