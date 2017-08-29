using UnityEngine;
using System.Collections;

public class HomingProjectile : MonoBehaviour 
{
	public GameObject target;
	public float speed;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target != null)
		{
			Vector3 toTarget = target.transform.position - transform.position;
			toTarget.Normalize();
			toTarget *= speed;
			transform.position += toTarget;
		}
	}
}
