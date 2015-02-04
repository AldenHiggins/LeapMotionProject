using UnityEngine;
using System.Collections;

public class DamageInRadius : MonoBehaviour 
{
	public int damage;
	public float radius;


	// Use this for initialization
	void Start ()
	{
		print ("Damaging in radius!!!!");
		Collider[] nearbyObjects = Physics.OverlapSphere (transform.position, radius);

		for (int i = 0; i < nearbyObjects.Length; i++)
		{
			if (nearbyObjects[i].transform.childCount > 0)
			{
				BasicEnemyController enemy = (BasicEnemyController) nearbyObjects[i].transform.GetChild(0).gameObject.GetComponent(typeof(BasicEnemyController));
				if (enemy != null)
				{
					print ("Radius dealing damage");
					enemy.dealDamage(damage);
				}
			}

		}
	}
}
