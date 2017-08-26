using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControls : MonoBehaviour
{
    private GameLogic game;
    private PlayerLogic player;

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
        player = GetObjects.getPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If the player is dead don't do anything
        if (!player.getIsAlive())
        {
            return;
        }

        // If we are still on the start screen wait for input and pass it along to the game
        if (game.getStartScreenActive())
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                game.startGame();
            }
            return;
        }

        // The round hasn't started yet and we are in the defensive setup mode
        if (!game.roundActive)
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                game.startRound();
            }

            placeDefenseAttack.holdFunction(OVRInput.Controller.RTouch);

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                placeDefenseAttack.releaseFunction(OVRInput.Controller.RTouch);
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                switchDefenseAttack.releaseFunction(OVRInput.Controller.RTouch);
            }
        }
        // The wave has started and enemies are still alive
        else
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                fireballAttack.releaseFunction(OVRInput.Controller.RTouch);
            }

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                fireballAttack.releaseFunction(OVRInput.Controller.LTouch);
            }
        }
	}
}
