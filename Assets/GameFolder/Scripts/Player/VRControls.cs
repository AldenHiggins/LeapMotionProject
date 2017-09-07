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

    // Keep track of the current update function and switch it out as the game switches state
    private delegate void UpdateFunction();
    private UpdateFunction currentUpdate;

    // Use this for initialization
    void Start ()
    {
        game = GetObjects.getGame();
        player = GetObjects.getPlayer();

        // Instantiate all of the attacks we're going to use
        placeDefenseAttack = Instantiate(placeDefenseAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        switchDefenseAttack = Instantiate(switchDefenseAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        fireballAttack = Instantiate(fireballAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();

        // Install our state-switching listeners
        EventManager.StartListening(GameEvents.DefensivePhaseStart, delegate() { currentUpdate = defensiveUpdate; });
        EventManager.StartListening(GameEvents.OffensivePhaseStart, delegate () { currentUpdate = offensiveUpdate; });

        // Start by waiting for input
        currentUpdate = waitForStartInputUpdate;
    }

    void offensiveUpdate()
    {
        return;

        //// Check for the user firing fireballs
        //if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        //{
        //    fireballAttack.releaseFunction(OVRInput.Controller.RTouch);
        //}
        //if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        //{
        //    fireballAttack.releaseFunction(OVRInput.Controller.LTouch);
        //}
    }

    void defensiveUpdate()
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

    void waitForStartInputUpdate()
    {
        // Wait for input to start the game
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown("v"))
        {
            EventManager.TriggerEvent(GameEvents.GameStart);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        currentUpdate();
	}
}
