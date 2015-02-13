using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour 
{

//	private Network

	public bool isServer;

	public GameLogic gameLogic;

	// Use this for initialization
	void Start ()
	{
		if (!isServer)
		{
			print ("Trying to connect!");
			Network.Connect("10.117.79.121", 80);
		}
		else
		{
			print ("Network IP: " + Network.player.ipAddress);

			Network.InitializeServer (22, 80, false);
			
//			print ("Server IP: " + Network.natFacilitatorIP);
//			print ("Server port: " + Network.natFacilitatorPort);
			// Make the server's avatar
			gameLogic.makePlayerOnClientHelper();
		}
	}

	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnPlayerConnected ()
	{
		print ("Player has connected to the server!");
		// Create a new avatar for this client
		gameLogic.createNewPlayer ();
	}

	void OnConnectedToServer()
	{
		print ("Connected to server!!!!");
	}
}
