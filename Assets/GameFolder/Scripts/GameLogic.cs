using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour 
{
	public GameObject avatarPrefab;
	public GameObject thisPlayer;
	public GameObject thisCamera;
	public GameObject fireBall;

	public HandController handController = null;

	private Dictionary<string, GameObject> playerAvatars; 
	private NetworkView view;
	private bool fireballCharged;

	private GameObject playerAvatar;
	// Use this for initialization
	void Start () 
	{
		view = gameObject.networkView;
		fireballCharged = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandModel[] hands = handController.GetAllGraphicsHands();
		if (hands.Length > 0)
		{
			Vector3 direction0 = (hands[0].GetPalmPosition() - handController.transform.position).normalized;
			Vector3 normal0 = hands[0].GetPalmNormal().normalized;

//			print ("Normal 0: " + normal0);
//			print ("Dot product: " + Vector3.Dot (normal0, thisCamera.transform.forward));

			//  -.6 or less means the palm is facing the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) < -.6 && !fireballCharged)
			{
				fireballCharged = true;
			}

			// .6 or more means the palm is facing away from the camera
			if (Vector3.Dot (normal0, thisCamera.transform.forward) > .6 && fireballCharged)
			{
				fireballCharged = false;
				print ("Fire fireball!!!");
				GameObject newFireball = (GameObject) Instantiate(fireBall, hands[0].GetPalmPosition(), thisCamera.transform.rotation);
				MoveFireball moveThis = (MoveFireball) newFireball.GetComponent(typeof(MoveFireball));
				moveThis.setVelocity(normal0);
			}
		}
//		else if (hands.Length > 1)
//		{
//			Vector3 direction0 = (hands[0].GetPalmPosition() - handController.transform.position).normalized;
//			Vector3 normal0 = hands[0].GetPalmNormal().normalized;
//			
//			Vector3 direction1 = (hands[1].GetPalmPosition() - handController.transform.position).normalized;
//			Vector3 normal1 = hands[1].GetPalmNormal().normalized;
//			
//			print ("Normal 0: " + normal0);
//			print ("Normal 1: " + normal1);
//
//		}
	}

	public void createNewPlayer ()
	{
		view.RPC ("makePlayerOnClient", RPCMode.Others);
	}

	[RPC]
	public void makePlayerOnClient ()
	{
		print ("Remote procedure called!");
		makePlayerOnClientHelper ();
	}

	public void makePlayerOnClientHelper()
	{
		// previously was Network.Instantiate
		playerAvatar = (GameObject) Network.Instantiate (avatarPrefab, thisPlayer.transform.position, thisPlayer.transform.rotation, 1);
		MoveAvatar avatar = (MoveAvatar) playerAvatar.GetComponent (typeof(MoveAvatar));
		avatar.setPlayer (thisPlayer);
		avatar.hidePlayer ();	
	}


}
