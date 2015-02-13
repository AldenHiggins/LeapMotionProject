using UnityEngine;
using System.Collections;

public class CircularParticleEmission : MonoBehaviour {

	public GameObject particle;
	public int angleDelta;
	// Use this for initialization
	void Start () 
	{
		int numberOfParticles = 360 / angleDelta;
		print ("Delta: " + angleDelta);
		print ("Number to spawn: " + numberOfParticles);

		for (int i = 0; i < numberOfParticles; i++)
		{
			Instantiate (particle, transform.position, Quaternion.Euler(0,i * angleDelta, 0));
		}
	}
}
