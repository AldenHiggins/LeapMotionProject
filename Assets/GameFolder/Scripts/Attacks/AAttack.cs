using System;
using UnityEngine;

public abstract class AAttack : MonoBehaviour
{
	abstract public void chargingFunction(HandModel[] hands);

	abstract public void chargedFunction(HandModel[] hands);

	abstract public void releaseFunction(HandModel[] hands);

	abstract public void holdGestureFunction(HandModel[] hands);

	abstract public void inactiveFunction(HandModel[] hands);
}


