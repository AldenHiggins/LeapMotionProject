using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControls : MonoBehaviour
{
    private GameLogic game;

    [SerializeField]
    private AAttack placeDefenseAttack;

    [SerializeField]
    private AAttack switchDefenseAttack;

    [SerializeField]
    private AAttack fireballAttack;

    [SerializeField]
    private AAttack handTriggerAttack;

    [SerializeField]
    private AAttack scaleChangeAttack;

    [SerializeField]
    private AAttack bButtonAttack;

    [SerializeField]
    private AAttack aButtonAttack;

    // Keep track of the current update function and switch it out as the game switches state
    private delegate void UpdateFunction();
    private UpdateFunction currentUpdate;
    private UpdateFunction alternateUpdate;

    // Control states
    private bool rHandTriggerDown;
    private bool lHandTriggerDown;

    // Use this for initialization
    void Start ()
    {
        game = GetObjects.getGame();

        // Instantiate all of the attacks we're going to use
        placeDefenseAttack = Instantiate(placeDefenseAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        switchDefenseAttack = Instantiate(switchDefenseAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        fireballAttack = Instantiate(fireballAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        handTriggerAttack = Instantiate(handTriggerAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        scaleChangeAttack = Instantiate(scaleChangeAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        bButtonAttack = Instantiate(bButtonAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();
        aButtonAttack = Instantiate(aButtonAttack.gameObject, GetObjects.getAttackContainer()).GetComponent<AAttack>();

        // Install our state-switching listeners
        EventManager.StartListening(GameEvents.DefensivePhaseStart, delegate() { currentUpdate = defensiveUpdate; });
        EventManager.StartListening(GameEvents.OffensivePhaseStart, delegate () { currentUpdate = offensiveUpdate; });

        // Start by waiting for input
        currentUpdate = waitForStartInputUpdate;

        // Always check the hand triggers and start button
        alternateUpdate = checkHandTriggersUpdate;
        alternateUpdate += checkForStartButtonUpdate;
        alternateUpdate += checkABButtonUpdate;
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

    void checkHandTriggersUpdate()
    {
        // Check if the user wants to switch the defense they're placing
        if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            rHandTriggerDown = true;
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger))
        {
            rHandTriggerDown = false;
            handTriggerAttack.releaseFunction(OVRInput.Controller.RTouch);
        }

        if (rHandTriggerDown)
        {
            handTriggerAttack.holdFunction(OVRInput.Controller.RTouch);
        }
    }

    void checkForStartButtonUpdate()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            scaleChangeAttack.releaseFunction(OVRInput.Controller.LTouch);
        }
    }

    void checkABButtonUpdate()
    {
        if (OVRInput.Get(OVRInput.RawButton.B))
        {
            bButtonAttack.holdFunction(OVRInput.Controller.RTouch);
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.B))
        {
            bButtonAttack.releaseFunction(OVRInput.Controller.RTouch);
        }

        if (OVRInput.Get(OVRInput.RawButton.A))
        {
            aButtonAttack.holdFunction(OVRInput.Controller.RTouch);
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            aButtonAttack.releaseFunction(OVRInput.Controller.RTouch);
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
        alternateUpdate();
	}
}
