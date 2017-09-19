using UnityEngine;
using System.Collections;

public class Ragdoll : MonoBehaviour 
{
	public GameObject rigidBodyPelvis;
	public float forceAmount;

	// Use this for initialization
	void Start () 
	{
		print ("Adding force");
		Rigidbody body = rigidBodyPelvis.GetComponent<Rigidbody> ();
		body.AddForce (transform.forward * forceAmount, ForceMode.Impulse);
		body.AddForce (Vector3.up * forceAmount / 2, ForceMode.Impulse);
//		StartCoroutine (delayedAddForce());
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	IEnumerator delayedAddForce()
	{
		yield return new WaitForSeconds (2);


	}
}
