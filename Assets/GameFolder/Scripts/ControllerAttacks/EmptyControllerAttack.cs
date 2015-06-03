using System;
using UnityEngine;


class EmptyControllerAttack : AControllerAttack
{
    public override void chargingFunction() { }

    public override void chargedFunction() { }

    public override void releaseFunction(GameObject camera) { }

    public override void holdFunction() { }

    public override void inactiveFunction() { }
}

