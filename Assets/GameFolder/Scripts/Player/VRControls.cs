using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControls : MonoBehaviour
{
    private GameLogic game;
    private PlayerLogic player;

    [SerializeField]
    private AAttack placeDefenseAttack;

    [SerializeField]
    private AAttack switchDefenseAttack;

    [SerializeField]
    private AAttack fireballAttack;

    // Use this for initialization
    void Start ()
    {
        game = GetObjects.getGame();
        player = GetObjects.getPlayer();

        // Instantiate all of the attacks we're going to use
        placeDefenseAttack = Instantiate(placeDefenseAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        switchDefenseAttack = Instantiate(switchDefenseAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        fireballAttack = Instantiate(fireballAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
    }

    // Update is called once per frame
    void Update ()
    {
        // If the player is dead don't do anything
        if (player.isUnitDying())
        {
            return;
        }

        // If we are still on the start screen wait for input and pass it along to the game
        if (game.getStartScreenActive())
        {
            if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown("v"))
            {
                game.startGame();
                EventManager.TriggerEvent(GameEvents.GameStart);
            }

            return;
        }

        // The round hasn't started yet and we are in the defensive setup mode
        if (!game.roundActive)
        {
            // Check for the user skipping the defensive stage and activating the wave
            if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown("v"))
            {
                game.startRound();
            }

            // Iterate the hold function of the place defense attack
            placeDefenseAttack.holdFunction(OVRInput.Controller.RTouch);

            // Check if the user wants to place a defense
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                placeDefenseAttack.releaseFunction(OVRInput.Controller.RTouch);
            }

            // Check if the user wants to switch the defense they're placing
            if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
            {
                switchDefenseAttack.releaseFunction(OVRInput.Controller.RTouch);
            }
        }
        // The wave has started and enemies are still alive
        else
        {
            // Check for the user firing fireballs
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                fireballAttack.releaseFunction(OVRInput.Controller.RTouch);
            }
            if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
            {
                fireballAttack.releaseFunction(OVRInput.Controller.LTouch);
            }
        }
	}
}
