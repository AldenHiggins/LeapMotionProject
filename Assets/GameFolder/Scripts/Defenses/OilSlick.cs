using UnityEngine;
using System.Collections;

public class OilSlick : MonoBehaviour
{
	public GameObject boom;
	private AudioSource source;
	public AudioClip walkingOnOil;

	// Use this for initialization
	void Start ()
    {
		source = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other) 
	{
		IUnit enemy = (IUnit) other.gameObject.GetComponent(typeof(IUnit));
		if (enemy != null && !enemy.isUnitAlly())
		{
			enemy.slowDown();
			source.PlayOneShot(walkingOnOil);
		}
	}

	public void blowUp()
	{
		Vector3 boomPos = transform.position + new Vector3(0f,1f,0f);
		Instantiate (boom, boomPos, Quaternion.identity);
		Destroy(gameObject);
	}
}
