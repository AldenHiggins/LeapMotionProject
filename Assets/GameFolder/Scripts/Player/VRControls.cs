using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VRControls : MonoBehaviour
{
    [SerializeField]
    private AAttack handTriggerAttack;

    [SerializeField]
    private AAttack startButtonAttack;

    [SerializeField]
    private AAttack bButtonAttack;

    [SerializeField]
    private AAttack aButtonAttack;

    // Keep track of the current update function
    private Action controlUpdate;

    // Control states
    private bool rHandTriggerDown;
    private bool lHandTriggerDown;

    // Use this for initialization
    void Start ()
    {
        // Get the container for the attacks we will instantiate
        Transform attackContainer = GetObjects.instance.getAttackContainer();
        
        // Check for the controls only for attacks that we have
        controlUpdate = noUpdate;
        if (handTriggerAttack)
        {
            handTriggerAttack = Instantiate(handTriggerAttack.gameObject, attackContainer).GetComponent<AAttack>();
            controlUpdate += checkHandTriggersUpdate;
        }
        if (startButtonAttack)
        {
            startButtonAttack = Instantiate(startButtonAttack.gameObject, attackContainer).GetComponent<AAttack>();
            controlUpdate += checkForStartButtonUpdate;
        }
        if (aButtonAttack && bButtonAttack)
        {
            bButtonAttack = Instantiate(bButtonAttack.gameObject, attackContainer).GetComponent<AAttack>();
            aButtonAttack = Instantiate(aButtonAttack.gameObject, attackContainer).GetComponent<AAttack>();
            controlUpdate += checkABButtonUpdate;
        }
    }

    void noUpdate() { }

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
}
