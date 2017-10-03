using UnityEngine;
using System.Collections;

public class DamageInRadius : MonoBehaviour 
{
	public int damage;
	public float radius;
	public float interval;
    public bool damageAllies;
    public bool damageOnce;

	// Use this for initialization
	void Start ()
	{
        StartCoroutine (damageOverInterval ());
	}

	IEnumerator damageOverInterval()
	{
		if (interval == 0)
		{
            dealDamageInRadius();
            yield break;
		}

		while (true)
		{
			yield return new WaitForSeconds(interval);
            dealDamageInRadius();
            if (damageOnce)
            {
                Destroy(this);
            }
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
                        Vector3 damageVector = enemy.getGameObject().transform.position - transform.position;
                        enemy.dealDamage(damage, damageVector.normalized);
                    }					
				}
			}
			
		}
	}
}
