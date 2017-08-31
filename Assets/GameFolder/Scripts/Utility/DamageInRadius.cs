using UnityEngine;
using System.Collections;

public class DamageInRadius : MonoBehaviour 
{
	public int damage;
	public float radius;
	public float interval;
    public bool damageAllies;

	// Use this for initialization
	void Start ()
	{
		dealDamageInRadius ();
		StartCoroutine (damageOverInterval ());
	}

	IEnumerator damageOverInterval()
	{
		if (interval == 0)
		{
            yield break;
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
				IUnit enemy = (IUnit) nearbyObjects[i].gameObject.GetComponent(typeof(IUnit));
				if (enemy != null)
				{
                    if (!damageAllies && enemy.isUnitAlly())
                    {

                    }
                    else
                    {
                        enemy.dealDamage(damage);
                    }					
				}
			}
			
		}
	}
}
