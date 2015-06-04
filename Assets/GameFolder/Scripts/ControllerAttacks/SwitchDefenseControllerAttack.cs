using System;
using UnityEngine;

class SwitchDefenseControllerAttack : AControllerAttack
{
    public GameObject defensiveChoices;
    public ControllerAbilities controlAbilities;

    private int currentAttackIndex;

    public override void chargingFunction() { }

    public override void chargedFunction() { }

    public override void releaseFunction(GameObject camera)
    {
        AControllerAttack previousAttack = (AControllerAttack) defensiveChoices.transform.GetChild(currentAttackIndex).gameObject.GetComponent(typeof(AControllerAttack));
        previousAttack.inactiveFunction();
        currentAttackIndex++;
        if (currentAttackIndex >= defensiveChoices.transform.childCount)
        {
            currentAttackIndex = 0;
        }

        controlAbilities.rightTriggerAttack = (AControllerAttack) defensiveChoices.transform.GetChild(currentAttackIndex).gameObject.GetComponent(typeof(AControllerAttack));
    }

    public override void holdFunction() { }

    public override void inactiveFunction() { }
}

