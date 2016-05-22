using UnityEngine;
using System.Collections;

public class SwordScript : MonoBehaviour
{
	public int damageAmount;

	void OnTriggerEnter(Collider other)
	{
		BasicEnemyController enemy = (BasicEnemyController) other.gameObject.GetComponent(typeof(BasicEnemyController));
		if (enemy == null)
		{
			enemy = (BasicEnemyController) other.gameObject.GetComponentInChildren(typeof(BasicEnemyController));
		}

		if (enemy != null && !enemy.isAlly)
		{
			enemy.dealDamage(damageAmount);
		}
	}
}

