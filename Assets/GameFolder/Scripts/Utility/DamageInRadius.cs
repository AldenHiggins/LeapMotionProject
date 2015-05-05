using UnityEngine;
using System.Collections;

public class DamageInRadius : MonoBehaviour 
{
	public int damage;
	public float radius;
	public float interval;


	// Use this for initialization
	void Start ()
	{


//		print ("Damaging in radius!!!!");
		dealDamageInRadius ();
		StartCoroutine (damageOverInterval ());
	}


	IEnumerator damageOverInterval()
	{
		if (interval == 0)
		{
			return true;
		}

		while (true)
		{
			yield return new WaitForSeconds(interval);
			dealDamageInRadius();
		}
	}

	void dealDamageInRadius()
	{
		Collider[] nearbyObjects = Physics.OverlapSphere (transform.position, radius);
		
		for (int i = 0; i < nearbyObjects.Length; i++)
		{
			if (nearbyObjects[i].transform.childCount > 0)
			{
				BasicEnemyController enemy = (BasicEnemyController) nearbyObjects[i].gameObject.GetComponent(typeof(BasicEnemyController));
				if (enemy != null)
				{
					//					print ("Radius dealing damage");
					enemy.dealDamage(damage);
				}
			}
			
		}
	}
}
