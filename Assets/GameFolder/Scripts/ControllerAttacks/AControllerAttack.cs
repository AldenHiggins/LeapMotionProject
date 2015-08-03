using UnityEngine;
using System.Collections;

public abstract class AControllerAttack : MonoBehaviour
{
	abstract public void chargingFunction();
	
	abstract public void chargedFunction();
	
	abstract public void releaseFunction(GameObject camera);
	
	abstract public void holdFunction();
	
	abstract public void inactiveFunction();
}
