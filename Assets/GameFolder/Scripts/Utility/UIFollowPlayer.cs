using UnityEngine;
using System.Collections;

public class UIFollowPlayer : MonoBehaviour {

	public GameObject player;


	private Vector3 initialOffset;
	// Use this for initialization
	void Start ()
	{
		initialOffset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = player.transform.position + initialOffset;
	}
}
