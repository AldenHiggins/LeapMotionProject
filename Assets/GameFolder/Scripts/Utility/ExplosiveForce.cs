using UnityEngine;
using System.Collections;

public class ExplosiveForce : MonoBehaviour 
{
	public float force;
	public float radius;
	
	
	// Use this for initialization
	void Start ()
	{
//		print ("Damaging in radius!!!!");
		Collider[] nearbyObjects = Physics.OverlapSphere (transform.position, radius);
		
		for (int i = 0; i < nearbyObjects.Length; i++)
		{
			if (nearbyObjects[i].transform.childCount > 0)
			{
				BasicEnemyController enemy = (BasicEnemyController) nearbyObjects[i].gameObject.GetComponent(typeof(BasicEnemyController));
				if (enemy != null)
				{
//					print ("Applying explosive force");
					enemy.applyExplosiveForce(force, transform.position, radius);
				}
			}
			
		}
	}
}
