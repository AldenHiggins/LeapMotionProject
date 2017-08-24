using UnityEngine;

public abstract class SteamAttacks : MonoBehaviour
{
	abstract public void inactiveFunction();

	abstract public void releaseFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice);

	abstract public void holdFunction(uint controllerIndex, SteamVR_TrackedObject trackedDevice);
}


