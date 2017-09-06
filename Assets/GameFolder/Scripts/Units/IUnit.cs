using UnityEngine;
using System;

public interface IUnit
{
    void installDeathListener(Action onDeathCallback);

    void installDamageListener(Action<int> onDamageCallback);

    int getMaxHealth();

    int getCurrentHealth();

	void dealDamage(int damageToDeal);

    bool isUnitDying();

    bool isUnitAlly();

    void slowDown();

	GameObject getGameObject();
}


