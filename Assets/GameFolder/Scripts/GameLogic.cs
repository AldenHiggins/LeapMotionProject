using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	public GameObject avatarPrefab;
	public GameObject thisPlayer;


	private Dictionary<string, GameObject> playerAvatars; 
	private NetworkView view;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void createNewPlayer ()
	{
		view.RPC ("MakePlayer", RPCMode.Others);
	}



	[RPC]
	public void MakePlayer ()
	{
		print ("Remote procedure called!");
	}


}
