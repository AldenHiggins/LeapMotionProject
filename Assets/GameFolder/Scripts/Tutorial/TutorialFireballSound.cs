using UnityEngine;
using System.Collections;

public class TutorialFireballSound : MonoBehaviour {

	public AudioSource source;
	public AudioClip sound;

	// Use this for initialization
	void Start () {
		StartCoroutine (playSound ());
	}

	IEnumerator playSound()
	{
		source.clip = sound;
		source.Play ();
		yield return new WaitForSeconds (source.clip.length);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
