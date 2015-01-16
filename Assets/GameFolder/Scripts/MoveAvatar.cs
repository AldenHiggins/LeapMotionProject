using UnityEngine;
using System.Collections;

public class MoveAvatar : MonoBehaviour 
{
	private GameObject thisPlayer;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (thisPlayer != null)
		{
			transform.position = thisPlayer.transform.position;
			transform.rotation = thisPlayer.transform.rotation;
		}
	}

	public void setPlayer(GameObject playerInput)
	{
		thisPlayer = playerInput;
	}
}
