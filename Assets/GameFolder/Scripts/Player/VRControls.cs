using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRControls : MonoBehaviour
{
    // List of all of the attacks
    [Header("Primary Attacks")]
    [SerializeField]
    private AAttack meleeAttack;
    [SerializeField]
    private AAttack shadowBallAttack;
    [SerializeField]
    private AAttack spikesAttack;
    [Header("Corpse Attacks")]
    [SerializeField]
    private AAttack corpseExplosionAttack;
    [SerializeField]
    private AAttack spawnSkeletonAttack;
    [Header("Command AI Attacks")]
    [SerializeField]
    private AAttack rallyAlliesAttack;
    [SerializeField]
    private AAttack commandAlliesAttack;
    [Header("Utilities")]
    [SerializeField]
    private AAttack pauseGameAttack;
    [SerializeField]
    private AAttack zoomInAttack;

    // Get references to the UI to select attacks
    [Header("Attack Selection UI")]
    [SerializeField]
    private ToggleGroup rightTriggerToggles;
    [SerializeField]
    private ToggleGroup leftTriggerToggles;

    // Currently assigned attacks to each of the controls
    private AAttack handTriggerAttack;
    private AAttack startButtonAttack;
    private AAttack bButtonAttack;
    private AAttack aButtonAttack;
    private AAttack rightTriggerAttack;
    private AAttack leftTriggerAttack;

    // Keep track of the current update function
    private Action controlUpdate;

    // Control states
    private bool rHandTriggerDown;
    private bool lHandTriggerDown;
    private bool rIndexTriggerDown;
    private bool lIndexTriggerDown;

    // Use this for initialization
    void Start ()
    {
        // Get the container for the attacks we will instantiate
        Transform attackContainer = GetObjects.instance.getAttackContainer();

        // Initialize the attacks
        initializeAttack(ref meleeAttack);
        initializeAttack(ref shadowBallAttack);
        initializeAttack(ref spikesAttack);
        initializeAttack(ref corpseExplosionAttack);
        initializeAttack(ref spawnSkeletonAttack);
        initializeAttack(ref rallyAlliesAttack);
        initializeAttack(ref commandAlliesAttack);
        initializeAttack(ref pauseGameAttack);
        initializeAttack(ref zoomInAttack);

        // Set default controls
        startButtonAttack = pauseGameAttack;
        bButtonAttack = spawnSkeletonAttack;
        aButtonAttack = corpseExplosionAttack;
        rightTriggerAttack = meleeAttack;
        leftTriggerAttack = shadowBallAttack;

        // Enable controls
        enableControls();

        // Only accept start button inputs while the game is paused, resume all input once the game resumes
        EventManager.StartListening(GameEvents.GamePause, delegate () { controlUpdate = checkForStartButtonUpdate; });
        EventManager.StartListening(GameEvents.GameResume, enableControls);
    }

    void initializeAttack(ref AAttack attackToInitialize)
    {
        if (attackToInitialize == null) return;
        attackToInitialize = Instantiate(attackToInitialize.gameObject, GetObjects.instance.getAttackContainer()).GetComponent<AAttack>();
    }

    void enableControls()
    {
        controlUpdate = noUpdate;
        controlUpdate += joysticksUpdate;
        if (handTriggerAttack)
        {
            controlUpdate += checkHandTriggersUpdate;
        }
        if (startButtonAttack)
        {
            controlUpdate += checkForStartButtonUpdate;
        }
        if (aButtonAttack && bButtonAttack)
        {
            controlUpdate += checkABButtonUpdate;
        }
        if (leftTriggerAttack && rightTriggerAttack)
        {
            controlUpdate += checkTriggersUpdate;
        }
    }

    void noUpdate() { }

    void joysticksUpdate()
    {
        // Make sure we can get the controllable unit
        ControllableUnit unit = GetObjects.instance.getControllableUnit();
        if (unit == null)
        {
            return;
        }
        // Get the camera to translate movement to the player's perspective
        GameObject playerCamera = GetObjects.instance.getCamera();

        // Take in input from the player
        Vector2 leftInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 rightInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Take in gamepad input if we don't have an OVR controller
        if (OVRInput.GetActiveController() == OVRInput.Controller.None)
        {
            leftInput = new Vector2(Input.GetAxis("Move_X_Axis"), Input.GetAxis("Move_Y_Axis"));
            rightInput = new Vector2(Input.GetAxis("Look_X_Axis"), Input.GetAxis("Look_Y_Axis"));
        }

        // Find the look vector of the unit
        Vector3 lookVector = new Vector3(rightInput.x, 0.0f, rightInput.y);
        // Find the movement vector of the unit
        Vector3 movementVector = new Vector3(leftInput.x, 0.0f, leftInput.y);
        // Determine whether the unit should strafe or not
        bool isStrafing = lookVector != Vector3.zero;

        // Figure out where the unit should look
        if (lookVector == Vector3.zero && movementVector == Vector3.zero)
        {
            lookVector = unit.transform.forward;
        }
        else if (lookVector == Vector3.zero)
        {
            lookVector = playerCamera.transform.rotation * movementVector;
        }
        else
        {
            lookVector = playerCamera.transform.rotation * lookVector;
        }

        lookVector.y = 0.0f;

        // Make the movement relative to the player's camera
        movementVector = playerCamera.transform.rotation * movementVector;
        movementVector.y = 0.0f;
        // Transform the movement into the controllable unit's space before passing it along
        movementVector = Quaternion.Inverse(unit.transform.rotation) * movementVector;

        // Update the unit's movement
        unit.movementUpdate(movementVector, lookVector, isStrafing);
    }

    void checkTriggersUpdate()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            rIndexTriggerDown = true;
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        {
            rIndexTriggerDown = false;
            rightTriggerAttack.releaseFunction(OVRInput.Controller.RTouch);
        }

        if (rIndexTriggerDown)
        {
            rightTriggerAttack.holdFunction(OVRInput.Controller.RTouch);
        }

        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            lIndexTriggerDown = true;
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
        {
            lIndexTriggerDown = false;
            leftTriggerAttack.releaseFunction(OVRInput.Controller.LTouch);
        }

        if (lIndexTriggerDown)
        {
            leftTriggerAttack.holdFunction(OVRInput.Controller.LTouch);
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
            startButtonAttack.releaseFunction(OVRInput.Controller.RTouch);
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

    // Update is called once per frame
    void Update ()
    {
        controlUpdate();
	}

    public void changeRightTriggerControl(int newAttackIndex)
    {
        rightTriggerAttack = aButtonAttack;
        leftTriggerAttack = bButtonAttack;
    }
}
