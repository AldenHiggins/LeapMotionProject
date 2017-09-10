using UnityEngine;
using System.Collections;

public class DamageOnCollision : MonoBehaviour
{
	public int damageAmount;

	void OnTriggerEnter(Collider other)
	{
		IUnit enemy = (IUnit) other.gameObject.GetComponent(typeof(IUnit));
		if (enemy == null)
		{
			enemy = (IUnit) other.gameObject.GetComponentInChildren(typeof(IUnit));
		}

		if (enemy != null && !enemy.isUnitAlly())
		{
            Vector3 damageVector = enemy.getGameObject().transform.position - transform.position;
			enemy.dealDamage(damageAmount, damageVector.normalized);
		}
	}
}

