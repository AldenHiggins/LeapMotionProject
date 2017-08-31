using UnityEngine;

public interface IUnit
{
	void dealDamage(int damageToDeal);

    bool isUnitDying();

    bool isUnitAlly();

    void slowDown();

	GameObject getGameObject();
}


