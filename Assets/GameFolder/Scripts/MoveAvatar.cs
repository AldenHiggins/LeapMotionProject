using UnityEngine;
using System.Collections;

public class MoveAvatar : MonoBehaviour 
{
	public GameObject thisPlayer;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = thisPlayer.transform.position;
		transform.rotation = thisPlayer.transform.rotation;
	}
}
