using UnityEngine;

public abstract class SteamAttacks : MonoBehaviour
{
    abstract public void inactiveFunction();

    abstract public void releaseFunction(OVRInput.Controller hand);

    abstract public void holdFunction(OVRInput.Controller hand);
}


