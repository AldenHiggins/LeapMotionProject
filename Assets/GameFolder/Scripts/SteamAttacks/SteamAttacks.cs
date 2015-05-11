using UnityEngine;

public abstract class SteamAttacks : MonoBehaviour
{
	abstract public void inactiveFunction();

	abstract public void releaseFunction(uint controllerIndex, GameObject trackedDevice);

	abstract public void holdFunction(uint controllerIndex, GameObject trackedDevice);
}


