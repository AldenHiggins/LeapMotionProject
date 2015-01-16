using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	public GameObject avatarPrefab;
	public GameObject thisPlayer;


	public GameObject camera;

	public GameObject rotate;

	private Dictionary<string, GameObject> playerAvatars; 
	private NetworkView view;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		camera.transform.position += new Vector3 (0.0f, 0.002f, 0.0f);
		rotate.transform.Rotate (new Vector3());
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
