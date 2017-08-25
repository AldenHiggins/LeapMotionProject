using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControls : MonoBehaviour
{
    private GameLogic game;

    [SerializeField]
    private SteamAttacks placeDefenseAttack;

    [SerializeField]
    private SteamAttacks switchDefenseAttack;

    [SerializeField]
    private SteamAttacks fireballAttack;


    // Use this for initialization
    void Start ()
    {
        game = GetObjects.getGame();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // The round hasn't started yet and we are in the defensive setup mode
        if (!game.roundActive)
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                Debug.Log("The player pressed ovr input button one");
                //OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                game.startRound();
            }

            placeDefenseAttack.holdFunction(OVRInput.Controller.RTouch);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Debug.Log("The trigger is gettin pressed");
                placeDefenseAttack.releaseFunction(OVRInput.Controller.RTouch);
            }

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                Debug.Log("The trigger is gettin pressed");
                switchDefenseAttack.releaseFunction(OVRInput.Controller.RTouch);
            }
        }
        // The wave has started and enemies are still alive
        else
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Debug.Log("The trigger is gettin pressed");
                fireballAttack.releaseFunction(OVRInput.Controller.RTouch);
            }

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                Debug.Log("The trigger is gettin pressed");
                fireballAttack.releaseFunction(OVRInput.Controller.LTouch);
            }
        }
	}
}
