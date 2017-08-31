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
			enemy.dealDamage(damageAmount);
		}
	}
}

