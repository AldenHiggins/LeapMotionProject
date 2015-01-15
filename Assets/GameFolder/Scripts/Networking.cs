using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {

//	private Network

	public bool isServer;

	// Use this for initialization
	void Start ()
	{
		if (!isServer)
		{
			print ("Trying to connect!");
			Network.Connect("10.96.10.42", 25000);
		}
		else
		{
			Network.InitializeServer (66, 25000, true);
			
			print ("Server IP: " + Network.natFacilitatorIP);
			print ("Server port: " + Network.natFacilitatorPort);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnConnectedToServer()
	{
		print ("Connected to server!!!!");
	}
}
