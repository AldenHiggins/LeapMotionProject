using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	public GameObject avatarPrefab;
	public GameObject thisPlayer;


	private Dictionary<string, GameObject> playerAvatars; 
	private NetworkView view;

	private GameObject playerAvatar;
	// Use this for initialization
	void Start () 
	{
		view = gameObject.networkView;
		makePlayerOnClient ();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

//	public void createNewPlayer ()
//	{
//		view.RPC ("MakePlayerOnClient", RPCMode.Others);
//	}
//
//	[RPC]
	public void makePlayerOnClient ()
	{
		print ("Remote procedure called!");
		playerAvatar = (GameObject) Network.Instantiate (avatarPrefab, thisPlayer.transform.position, thisPlayer.transform.rotation, 1);
		MoveAvatar avatar = (MoveAvatar) playerAvatar.GetComponent (typeof(MoveAvatar));
		avatar.setPlayer (thisPlayer);
		avatar.hidePlayer ();	
	}


}
