using UnityEngine;
using System.Collections;

public class ProjectileDestroy : MonoBehaviour 
{
	public float lifeSpan;
	
	public void Start() 
	{
		StartCoroutine (selfDestruct ());
	}
	
	IEnumerator selfDestruct()
	{
		yield return new WaitForSeconds (lifeSpan);
		if (this.gameObject != null)
			Destroy (this.gameObject);
	}
}
