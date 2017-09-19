using UnityEngine;
using System;

public interface IUnit
{
    void installDeathListener(Action onDeathCallback);

    void installDamageListener(Action<int, Vector3> onDamageCallback);

    int getMaxHealth();

    int getCurrentHealth();

	void dealDamage(int damageToDeal, Vector3 damageDirection);

    bool isUnitDying();

    bool isUnitAlly();

    void slowDown();

	GameObject getGameObject();
}


